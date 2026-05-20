using Domain.Models;
using Infrastructure.Adapters.Persistence;

namespace Infrastructure.Mappers.Interface;

public interface IAppointmentMapper
{
    AppointmentEntity ToEntity(Appointment appointment);

    Appointment ToDomain(AppointmentEntity entity);
}