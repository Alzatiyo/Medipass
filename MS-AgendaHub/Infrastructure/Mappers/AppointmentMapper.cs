using Domain.Models;
using Infrastructure.Adapters.Persistence;
using Infrastructure.Mappers.Interface;

namespace Infrastructure.Mappers;

public class AppointmentMapper : IAppointmentMapper
{
    public AppointmentEntity ToEntity(
        Appointment appointment)
    {
        return new AppointmentEntity
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            Specialty = appointment.Specialty,
            AppointmentDate = appointment.AppointmentDate,
            ProcedureCode = appointment.ProcedureCode,
            Status = appointment.Status,
            CreatedAt = appointment.CreatedAt
        };
    }

    public Appointment ToDomain(
        AppointmentEntity entity)
    {
        return new Appointment
        {
            Id = entity.Id,
            PatientId = entity.PatientId,
            DoctorId = entity.DoctorId,
            Specialty = entity.Specialty,
            AppointmentDate = entity.AppointmentDate,
            ProcedureCode = entity.ProcedureCode,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }
}