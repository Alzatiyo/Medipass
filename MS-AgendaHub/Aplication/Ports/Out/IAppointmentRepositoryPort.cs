using Domain.Models;
namespace Aplication.Ports.Out;
public interface IAppointmentRepositoryPort
{
    Task SaveAsync(Appointment appointment);

    Task<bool> ExistsAppointmentAsync(Guid doctorId, DateTime appointmentDate);
}