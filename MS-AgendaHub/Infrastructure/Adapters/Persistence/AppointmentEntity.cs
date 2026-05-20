using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Adapters.Persistence;

public class AppointmentEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public string Specialty { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }

    public string ProcedureCode { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}