namespace MsEhrLogger.Domain.Enums;

/// <summary>
/// Tipos de eventos que puede registrar el EHR Logger.
/// Espejo de MedicalSpecialty.cs del MS-AgendaHub.
/// </summary>
public enum EhrEventType
{
    AppointmentConfirmed,
    AppointmentCancelled,
    AppointmentRescheduled,
    PatientUpdated
}
