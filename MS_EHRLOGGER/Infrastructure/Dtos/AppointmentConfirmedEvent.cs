namespace MsEhrLogger.Infrastructure.Dtos;

/// <summary>
/// Evento publicado por MS-AgendaHub (EhrEventPublisherAdapter.cs)
/// al confirmar una cita. Es el contrato del mensaje en la cola RabbitMQ.
///
/// Los nombres de campo coinciden exactamente con los que publica
/// EhrEventPublisherAdapter.cs del MS-AgendaHub para garantizar
/// la deserialización correcta.
/// </summary>
public class AppointmentConfirmedEvent
{
    /// <summary>ID único del mensaje (para idempotencia).</summary>
    public string   EventId            { get; set; } = string.Empty;

    public string   AppointmentId      { get; set; } = string.Empty;
    public string   PatientId          { get; set; } = string.Empty;
    public string   DoctorId           { get; set; } = string.Empty;
    public string   Specialty          { get; set; } = string.Empty;
    public DateTime ScheduledAt        { get; set; }
    public string   ConsultationRoom   { get; set; } = string.Empty;
    public string   InsuranceCode      { get; set; } = string.Empty;
    public string   ProcedureCode      { get; set; } = string.Empty;
    public bool     InsuranceValidated { get; set; }
    public string   Observations       { get; set; } = string.Empty;

    /// <summary>ID de correlación para trazabilidad distribuida (Jaeger).</summary>
    public string   CorrelationId      { get; set; } = string.Empty;
    public string   SourceService      { get; set; } = string.Empty;
    public DateTime PublishedAt        { get; set; }
}
