using Domain.Models;
namespace Aplication.Ports.In;
public interface IAppointmentUseCasePort
{
    Task<Appointment> ScheduleAppointmentAsync(Appointment appointment);
}