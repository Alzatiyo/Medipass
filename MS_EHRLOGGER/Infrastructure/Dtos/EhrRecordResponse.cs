using MsEhrLogger.Domain.Enums;

namespace MsEhrLogger.Infrastructure.Dtos;

/// <summary>
/// DTO de respuesta para la API REST.
/// Espejo de CreateAppointmentRequest.cs / UpdateAppointmentStatusRequest.cs
/// del MS-AgendaHub — desacopla el dominio de la representación HTTP.
/// </summary>
public class EhrRecordResponse
{
    public string          Id                 { get; set; } = string.Empty;
    public string          AppointmentId      { get; set; } = string.Empty;
    public string          PatientId          { get; set; } = string.Empty;
    public string          DoctorId           { get; set; } = string.Empty;
    public string          Specialty          { get; set; } = string.Empty;
    public EhrEventType    EventType          { get; set; }
    public EhrRecordStatus Status             { get; set; }
    public AppointmentSummaryDto? AppointmentSummary { get; set; }
    public DateTime        OccurredAt         { get; set; }
    public DateTime?       ProcessedAt        { get; set; }
    public string          SourceService      { get; set; } = string.Empty;
    public string          CorrelationId      { get; set; } = string.Empty;
    public int             RetryCount         { get; set; }
}

public class AppointmentSummaryDto
{
    public DateTime ScheduledAt        { get; set; }
    public string   ConsultationRoom   { get; set; } = string.Empty;
    public string   InsuranceCode      { get; set; } = string.Empty;
    public string   ProcedureCode      { get; set; } = string.Empty;
    public string   Observations       { get; set; } = string.Empty;
    public bool     InsuranceValidated { get; set; }
}
