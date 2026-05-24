using Domain.Exceptions;
using Domain.Models;

namespace Domain.Services;

public class AppointmentService
{
    // Validar si la cita tiene una fecha válida
    public void ValidateAppointmentDate(Appointment appointment)
    {
        if (appointment.AppointmentDate <= DateTime.UtcNow)
        {
            throw new DomainException(
                "La fecha de la cita debe ser futura.");
        }
    }

    // Validar especialidad médica
    public void ValidateSpecialty(Appointment appointment)
    {
        if (string.IsNullOrWhiteSpace(appointment.Specialty))
        {
            throw new DomainException(
                "La especialidad médica es obligatoria.");
        }
    }

    // Validar procedimiento
    public void ValidateProcedure(Appointment appointment)
    {
        if (string.IsNullOrWhiteSpace(appointment.ProcedureCode))
        {
            throw new DomainException(
                "El código del procedimiento es obligatorio.");
        }
    }

    // Ejecutar todas las validaciones
    public void Validate(Appointment appointment)
    {
        ValidateAppointmentDate(appointment);

        ValidateSpecialty(appointment);

        ValidateProcedure(appointment);
    }
}