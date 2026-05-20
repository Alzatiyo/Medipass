using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Adapters.Persistence;

public class DoctorEntity
{
    [Key]
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;
}