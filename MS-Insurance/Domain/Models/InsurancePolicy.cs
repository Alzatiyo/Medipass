using Domain.Enums;

namespace Domain.Models;

public class InsurancePolicy
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string InsuranceNumber { get; set; } = string.Empty;

    public string PatientName { get; set; } = string.Empty;

    public PlanType Plan { get; set; }

    public bool IsActive { get; set; }

    public DateTime ExpirationDate { get; set; }
}
