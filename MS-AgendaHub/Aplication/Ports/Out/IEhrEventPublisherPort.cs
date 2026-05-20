using Domain.Models;
namespace Aplication.Ports.Out;
public interface IEhrEventPublisherPort
{
    Task PublishAppointmentAsync(Appointment appointment);
}