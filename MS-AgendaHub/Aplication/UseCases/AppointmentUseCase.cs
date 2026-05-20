using Aplication.Ports.In;
using Aplication.Ports.Out;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
namespace Aplication.UseCases;


public class AppointmentUseCase : IAppointmentUseCasePort
{
    private readonly IAppointmentRepositoryPort _repository;
    private readonly IInsuranceServicePort _insuranceService;
    private readonly IEhrEventPublisherPort _ehrPublisher;

public AppointmentUseCase(
IAppointmentRepositoryPort repository,
IInsuranceServicePort insuranceService,
IEhrEventPublisherPort ehrPublisher)
    {
        _repository = repository;
        _insuranceService = insuranceService;
        _ehrPublisher = ehrPublisher;
    }
    public async Task<Appointment> ScheduleAppointmentAsync(Appointment
    appointment)
    {
        // Validar cobertura del procedimiento con la aseguradora
        var insuranceResult = await _insuranceService.ValidateProcedureAsync("INS-001",appointment.ProcedureCode);
        // Regla de negocio:
        // No permitir agendar si el procedimiento no está cubierto
        if (insuranceResult == InsuranceStatus.ProcedureNotCovered)
        {
            throw new DomainException(
            "La aseguradora indicó que el procedimiento no está cubierto.");
        }
        // Validar disponibilidad del médico
        var exists = await _repository.ExistsAppointmentAsync(
        appointment.DoctorId,
        appointment.AppointmentDate);
        // Regla de negocio:
        // Evitar doble agendamiento
        if (exists)
        {
            throw new DomainException(
            "El médico ya tiene una cita asignada en este horario.");
        }
        // Guardar la cita
        await _repository.SaveAsync(appointment);
        // Enviar evento asíncrono al historial clínico
        _ = Task.Run(async () =>
        {
            await _ehrPublisher.PublishAppointmentAsync(appointment);
        });
        
    return appointment;
    }
}