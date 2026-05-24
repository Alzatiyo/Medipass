namespace Domain.Models;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public string Specialty { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }

    public string ProcedureCode { get; set; } = string.Empty;

    public string Status { get; set; } = "CONFIRMED";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}