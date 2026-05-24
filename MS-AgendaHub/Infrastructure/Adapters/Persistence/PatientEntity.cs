using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Adapters.Persistence;

public class PatientEntity
{
    [Key]
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string InsuranceNumber { get; set; } = string.Empty;
}