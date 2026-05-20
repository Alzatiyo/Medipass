namespace Domain.Models;

public class Doctor
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;
}