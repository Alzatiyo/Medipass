using Domain.Models;

namespace Domain.Builders;

public class AppointmentBuilder
{
    private readonly Appointment _appointment;

    public AppointmentBuilder()
    {
        _appointment = new Appointment();
    }

    public AppointmentBuilder WithPatient(Guid patientId)
    {
        _appointment.PatientId = patientId;
        return this;
    }

    public AppointmentBuilder WithDoctor(Guid doctorId)
    {
        _appointment.DoctorId = doctorId;
        return this;
    }

    public AppointmentBuilder WithSpecialty(string specialty)
    {
        _appointment.Specialty = specialty;
        return this;
    }

    public AppointmentBuilder WithDate(DateTime appointmentDate)
    {
        _appointment.AppointmentDate = appointmentDate;
        return this;
    }

    public AppointmentBuilder WithProcedure(string procedureCode)
    {
        _appointment.ProcedureCode = procedureCode;
        return this;
    }

    public Appointment Build()
    {
        return _appointment;
    }
}