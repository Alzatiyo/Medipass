using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Adapters.Persistence;

public class InsurancePolicyEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string InsuranceNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string PatientName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Plan { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime ExpirationDate { get; set; }
}
