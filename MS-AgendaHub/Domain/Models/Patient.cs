namespace Domain.Models;

public class Patient
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string InsuranceNumber { get; set; } = string.Empty;
}