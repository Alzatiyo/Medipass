using MsEhrLogger.Domain.Enums;
using MsEhrLogger.Domain.Exceptions;

namespace MsEhrLogger.Domain.Models;

/// <summary>
/// Aggregate Root del dominio EHR Logger.
/// Representa un registro inmutable en el Historial Clínico Digital.
///
/// Espejo de Appointment.cs del MS-AgendaHub.
///
/// SOLID:
///   SRP — solo encapsula estado e invariantes del registro EHR.
///   OCP — extensible por nuevos EhrEventType sin modificar esta clase.
/// </summary>
public class EhrRecord
{
    // ── Identidad ──────────────────────────────────────────────────────────
    public string           Id              { get; private set; } = string.Empty;
    public string           AppointmentId   { get; private set; } = string.Empty;
    public string           PatientId       { get; private set; } = string.Empty;
    public string           DoctorId        { get; private set; } = string.Empty;
    public string           Specialty       { get; private set; } = string.Empty;

    // ── Estado ─────────────────────────────────────────────────────────────
    public EhrEventType     EventType       { get; private set; }
    public EhrRecordStatus  Status          { get; private set; }
    public AppointmentSummary AppointmentSummary { get; private set; } = new();

    // ── Auditoría ──────────────────────────────────────────────────────────
    public DateTime  OccurredAt     { get; private set; }
    public DateTime? ProcessedAt    { get; private set; }
    public string    SourceService  { get; private set; } = string.Empty;
    public string    CorrelationId  { get; private set; } = string.Empty;
    public int       RetryCount     { get; private set; }

    // Constructor privado — solo accesible desde el Builder (patrón GoF)
    private EhrRecord() { }

    // ── Factory Method (patrón GoF) ────────────────────────────────────────

    /// <summary>
    /// Crea un EhrRecord válido a partir de un evento de cita confirmada.
    /// Valida las invariantes de dominio antes de construir.
    /// Regla de Negocio #3: el resumen llega de forma asíncrona desde MS-AgendaHub.
    /// </summary>
    public static EhrRecord CreateFromAppointmentConfirmed(
        string appointmentId,
        string patientId,
        string doctorId,
        string specialty,
        AppointmentSummary appointmentSummary,
        string correlationId,
        string sourceService)
    {
        ValidateMandatoryFields(appointmentId, patientId, doctorId, specialty, correlationId);

        return new EhrRecord
        {
            Id                 = Guid.NewGuid().ToString(),
            AppointmentId      = appointmentId,
            PatientId          = patientId,
            DoctorId           = doctorId,
            Specialty          = specialty,
            EventType          = EhrEventType.AppointmentConfirmed,
            Status             = EhrRecordStatus.Pending,
            AppointmentSummary = appointmentSummary,
            OccurredAt         = DateTime.UtcNow,
            ProcessedAt        = null,
            SourceService      = sourceService,
            CorrelationId      = correlationId,
            RetryCount         = 0
        };
    }

    // ── Transiciones de estado ─────────────────────────────────────────────

    /// <summary>
    /// Regla de Negocio: marca el registro como almacenado exitosamente.
    /// </summary>
    public void MarkAsStored()
    {
        if (Status == EhrRecordStatus.Stored)
            throw new InvalidEhrRecordException(
                $"El registro EHR [{Id}] ya fue almacenado previamente.");

        Status      = EhrRecordStatus.Stored;
        ProcessedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Registra un intento fallido e incrementa el contador de reintentos.
    /// Cuando supera maxRetries pasa a DeadLetter.
    /// </summary>
    public void MarkAsFailed(int maxRetries = 3)
    {
        RetryCount++;
        Status = RetryCount >= maxRetries
            ? EhrRecordStatus.DeadLetter
            : EhrRecordStatus.Failed;
    }

    public bool IsDeadLetter() => Status == EhrRecordStatus.DeadLetter;

    // ── Reconstitución desde persistencia (para el Mapper) ────────────────

    /// <summary>
    /// Reconstruye un EhrRecord desde la base de datos sin validar invariantes.
    /// Solo debe ser llamado por el EhrRecordMapper (capa de infraestructura).
    /// </summary>
    internal static EhrRecord Reconstitute(
        string id, string appointmentId, string patientId,
        string doctorId, string specialty,
        EhrEventType eventType, EhrRecordStatus status,
        AppointmentSummary summary,
        DateTime occurredAt, DateTime? processedAt,
        string sourceService, string correlationId, int retryCount)
    {
        return new EhrRecord
        {
            Id                 = id,
            AppointmentId      = appointmentId,
            PatientId          = patientId,
            DoctorId           = doctorId,
            Specialty          = specialty,
            EventType          = eventType,
            Status             = status,
            AppointmentSummary = summary,
            OccurredAt         = occurredAt,
            ProcessedAt        = processedAt,
            SourceService      = sourceService,
            CorrelationId      = correlationId,
            RetryCount         = retryCount
        };
    }

    // ── Validaciones de dominio ────────────────────────────────────────────

    private static void ValidateMandatoryFields(
        string appointmentId, string patientId,
        string doctorId, string specialty, string correlationId)
    {
        if (string.IsNullOrWhiteSpace(appointmentId))
            throw new InvalidEhrRecordException("AppointmentId es obligatorio.");
        if (string.IsNullOrWhiteSpace(patientId))
            throw new InvalidEhrRecordException("PatientId es obligatorio.");
        if (string.IsNullOrWhiteSpace(doctorId))
            throw new InvalidEhrRecordException("DoctorId es obligatorio.");
        if (string.IsNullOrWhiteSpace(specialty))
            throw new InvalidEhrRecordException("Specialty es obligatorio.");
        if (string.IsNullOrWhiteSpace(correlationId))
            throw new InvalidEhrRecordException("CorrelationId es obligatorio para trazabilidad.");
    }
}
