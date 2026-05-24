namespace MsEhrLogger.Domain.Enums;

/// <summary>
/// Estados posibles de un registro en el Historial Clínico Digital.
/// Espejo de AppointmentStatus.cs del MS-AgendaHub.
/// </summary>
public enum EhrRecordStatus
{
    Pending,
    Stored,
    Failed,
    DeadLetter
}
