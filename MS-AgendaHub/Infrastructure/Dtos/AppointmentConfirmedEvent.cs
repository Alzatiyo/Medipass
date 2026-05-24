namespace Infrastructure.Dtos;

/// <summary>
/// DTO que representa el evento publicado hacia RabbitMQ.
/// Sus campos deben coincidir 1:1 con AppointmentConfirmedEvent.cs
/// del MS-EHRLogger para garantizar la deserialización correcta.
///
/// ⚠️  Cualquier cambio en este archivo debe replicarse en MS-EHRLogger.
/// </summary>
public class AppointmentConfirmedEvent
{
    /// <summary>ID único del mensaje — usado por MS-EHRLogger para idempotencia.</summary>
    public string   EventId            { get; set; } = string.Empty;
    public string   AppointmentId      { get; set; } = string.Empty;
    public string   PatientId          { get; set; } = string.Empty;
    public string   DoctorId           { get; set; } = string.Empty;

    /// <summary>Debe enviarse como string (ej: "Oncología"), no como int del enum.</summary>
    public string   Specialty          { get; set; } = string.Empty;
    public DateTime ScheduledAt        { get; set; }
    public string   ConsultationRoom   { get; set; } = string.Empty;
    public string   InsuranceCode      { get; set; } = string.Empty;
    public string   ProcedureCode      { get; set; } = string.Empty;
    public bool     InsuranceValidated { get; set; }
    public string   Observations       { get; set; } = string.Empty;

    /// <summary>Propagado desde la petición HTTP para trazabilidad en Jaeger.</summary>
    public string   CorrelationId      { get; set; } = string.Empty;
    public string   SourceService      { get; set; } = "ms-agendahub";
    public DateTime PublishedAt        { get; set; }
}
