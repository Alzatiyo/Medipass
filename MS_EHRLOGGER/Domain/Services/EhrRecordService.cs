using MsEhrLogger.Domain.Builders;
using MsEhrLogger.Domain.Models;

namespace MsEhrLogger.Domain.Services;

/// <summary>
/// Servicio de dominio: encapsula la lógica de negocio pura del EHR Logger.
/// Espejo de AppointmentService.cs del MS-AgendaHub.
///
/// Sin dependencias de infraestructura — solo modelos y builders del dominio.
/// </summary>
public class EhrRecordService
{
    private const int MaxRetries = 3;

    /// <summary>
    /// Construye un EhrRecord usando el Builder y valida las invariantes de dominio.
    /// Regla de Negocio #3: el historial se actualiza sin demorar la llamada de la cita.
    /// </summary>
    public EhrRecord BuildPendingRecord(
        string appointmentId,
        string patientId,
        string doctorId,
        string specialty,
        AppointmentSummary summary,
        string correlationId,
        string sourceService)
    {
        return new EhrRecordBuilder()
            .WithAppointmentId(appointmentId)
            .WithPatientId(patientId)
            .WithDoctorId(doctorId)
            .WithSpecialty(specialty)
            .WithAppointmentSummary(summary)
            .WithCorrelationId(correlationId)
            .WithSourceService(sourceService)
            .Build();
    }

    /// <summary>
    /// Determina si un registro puede reintentarse o debe ir a Dead Letter.
    /// </summary>
    public bool CanRetry(EhrRecord record) =>
        record.RetryCount < MaxRetries && !record.IsDeadLetter();
}
