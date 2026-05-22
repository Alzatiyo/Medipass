namespace MsEhrLogger.Domain.Models;

/// <summary>
/// Value Object: resumen de la cita publicado por MS-AgendaHub.
/// Sus campos coinciden 1:1 con el evento que publica EhrEventPublisherAdapter.cs.
/// </summary>
public class AppointmentSummary
{
    public DateTime ScheduledAt       { get; init; }
    public string   ConsultationRoom  { get; init; } = string.Empty;
    public string   InsuranceCode     { get; init; } = string.Empty;
    public string   ProcedureCode     { get; init; } = string.Empty;
    public string   Observations      { get; init; } = string.Empty;
    public bool     InsuranceValidated { get; init; }
}
