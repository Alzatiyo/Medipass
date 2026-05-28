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
    public async Task<Appointment> ScheduleAppointmentAsync(Appointment appointment)
    {
        // Obtener el nmero de seguro del paciente
        var insuranceNumber = await _repository.GetPatientInsuranceNumberAsync(appointment.PatientId);
        if (string.IsNullOrEmpty(insuranceNumber))
        {
            throw new DomainException("El paciente no existe o no tiene un seguro mdico registrado.");
        }

        // Validar cobertura del procedimiento con la aseguradora
        var insuranceResult = await _insuranceService.ValidateProcedureAsync(insuranceNumber, appointment.ProcedureCode);
        // Regla de negocio:
        // No permitir agendar si el procedimiento no está cubierto
        if (insuranceResult == InsuranceStatus.ProcedureNotCovered)
        {
            throw new DomainException(
            "La aseguradora indicó que el procedimiento no está cubierto.");
        }
        
        // Regla de negocio:
        // No permitir agendar si la póliza está inactiva o vencida
        if (insuranceResult == InsuranceStatus.Rejected)
        {
            throw new DomainException(
            "La póliza de seguro del paciente se encuentra inactiva, vencida o no es válida.");
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