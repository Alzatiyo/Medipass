namespace Infrastructure.Dtos;

public class ValidateInsuranceResponse
{
    public bool Approved { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}
