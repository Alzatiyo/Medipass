namespace Infrastructure.Dtos;

public class CreateAppointmentRequest
{
    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public string Specialty { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }

    public string ProcedureCode { get; set; } = string.Empty;
}