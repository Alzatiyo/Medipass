using Aplication.Observability;
using Aplication.Ports.In;
using Aplication.Ports.Out;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using System.Diagnostics;

namespace Aplication.UseCases;

public class AppointmentUseCase : IAppointmentUseCasePort
{
    private readonly IAppointmentRepositoryPort _repository;
    private readonly IInsuranceServicePort      _insuranceService;
    private readonly IEhrEventPublisherPort     _ehrPublisher;

    public AppointmentUseCase(
        IAppointmentRepositoryPort repository,
        IInsuranceServicePort      insuranceService,
        IEhrEventPublisherPort     ehrPublisher)
    {
        _repository       = repository;
        _insuranceService = insuranceService;
        _ehrPublisher     = ehrPublisher;
    }

    public async Task<Appointment> ScheduleAppointmentAsync(Appointment appointment)
    {
        // Obtener el número de seguro del paciente
        var insuranceNumber = await _repository.GetPatientInsuranceNumberAsync(appointment.PatientId);
        if (string.IsNullOrEmpty(insuranceNumber))
        {
            throw new DomainException("El paciente no existe o no tiene un seguro médico registrado.");
        }

        InsuranceStatus insuranceResult;
        var sw = Stopwatch.StartNew();
        try
        {
            // Validar cobertura del procedimiento con la aseguradora
            insuranceResult = await _insuranceService.ValidateProcedureAsync(
                insuranceNumber, appointment.ProcedureCode);
        }
        finally
        {
            sw.Stop();
            MedipassMetrics.InsuranceCallDuration.Observe(sw.Elapsed.TotalSeconds);
        }

        if (insuranceResult == InsuranceStatus.ProcedureNotCovered)
        {
            MedipassMetrics.InsuranceRejections.Inc();
            MedipassMetrics.AppointmentsRejected.WithLabels("insurance_rejected").Inc();
            throw new DomainException(
                "La aseguradora indicó que el procedimiento no está cubierto.");
        }
        
        // Regla de negocio:
        // No permitir agendar si la póliza está inactiva o vencida
        if (insuranceResult == InsuranceStatus.Rejected)
        {
            MedipassMetrics.InsuranceRejections.Inc();
            MedipassMetrics.AppointmentsRejected.WithLabels("insurance_rejected").Inc();
            throw new DomainException(
            "La póliza de seguro del paciente se encuentra inactiva, vencida o no es válida.");
        }
        
        // Validar disponibilidad del médico
        var exists = await _repository.ExistsAppointmentAsync(
            appointment.DoctorId,
            appointment.AppointmentDate);

        if (exists)
        {
            MedipassMetrics.DoubleBookingBlocked.Inc();
            MedipassMetrics.AppointmentsRejected.WithLabels("double_booking").Inc();
            throw new DomainException(
                "El médico ya tiene una cita asignada en este horario.");
        }

        await _repository.SaveAsync(appointment);
        MedipassMetrics.AppointmentsConfirmed.WithLabels(appointment.Specialty.ToString()).Inc();

        _ = Task.Run(async () =>
        {
            try
            {
                await _ehrPublisher.PublishAppointmentAsync(appointment);
                MedipassMetrics.EhrEventsPublished.Inc();
            }
            catch (Exception ex)
            {
                MedipassMetrics.EhrPublishFailures.Inc();
                Console.Error.WriteLine(
                    $"[EHR-PUBLISH-FAILURE] No se pudo publicar evento para cita {appointment.Id}: {ex.Message}");
            }
        });

        return appointment;
    }
}