namespace Aplication.Ports.Out;
public interface IDoctorAvailabilityPort
{
    Task<bool> IsDoctorAvailableAsync(
    Guid doctorId,
    DateTime appointmentDate);
}