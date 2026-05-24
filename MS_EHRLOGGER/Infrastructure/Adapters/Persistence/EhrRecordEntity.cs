using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MsEhrLogger.Domain.Enums;

namespace MsEhrLogger.Infrastructure.Adapters.Persistence;

/// <summary>
/// Entidad de persistencia MongoDB.
/// Espejo de AppointmentEntity.cs / DoctorEntity.cs del MS-AgendaHub.
/// Separada del modelo de dominio (Anti-Corruption Layer).
/// </summary>
[BsonIgnoreExtraElements]
public class EhrRecordEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("appointmentId")]
    public string AppointmentId { get; set; } = string.Empty;

    [BsonElement("patientId")]
    public string PatientId { get; set; } = string.Empty;

    [BsonElement("doctorId")]
    public string DoctorId { get; set; } = string.Empty;

    [BsonElement("specialty")]
    public string Specialty { get; set; } = string.Empty;

    [BsonElement("eventType")]
    [BsonRepresentation(BsonType.String)]
    public EhrEventType EventType { get; set; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public EhrRecordStatus Status { get; set; }

    [BsonElement("appointmentSummary")]
    public AppointmentSummaryDocument? AppointmentSummary { get; set; }

    [BsonElement("occurredAt")]
    public DateTime OccurredAt { get; set; }

    [BsonElement("processedAt")]
    public DateTime? ProcessedAt { get; set; }

    [BsonElement("sourceService")]
    public string SourceService { get; set; } = string.Empty;

    [BsonElement("correlationId")]
    public string CorrelationId { get; set; } = string.Empty;

    [BsonElement("retryCount")]
    public int RetryCount { get; set; }
}

/// <summary>Documento embebido del resumen de cita.</summary>
[BsonIgnoreExtraElements]
public class AppointmentSummaryDocument
{
    [BsonElement("scheduledAt")]     public DateTime ScheduledAt       { get; set; }
    [BsonElement("consultationRoom")] public string  ConsultationRoom  { get; set; } = string.Empty;
    [BsonElement("insuranceCode")]   public string   InsuranceCode     { get; set; } = string.Empty;
    [BsonElement("procedureCode")]   public string   ProcedureCode     { get; set; } = string.Empty;
    [BsonElement("observations")]    public string   Observations      { get; set; } = string.Empty;
    [BsonElement("insuranceValidated")] public bool  InsuranceValidated { get; set; }
}
