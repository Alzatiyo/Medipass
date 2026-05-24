using Aplication.Ports.Out;

namespace Infrastructure.Adapters.Rest;

public class DoctorAvailabilityAdapter
    : IDoctorAvailabilityPort
{
    public async Task<bool> IsDoctorAvailableAsync(
        Guid doctorId,
        DateTime appointmentDate)
    {
        // Simulación de validación externa

        await Task.Delay(300);

        return true;
    }
}