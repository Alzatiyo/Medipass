namespace Infrastructure.Dtos;

public class ValidateInsuranceRequest
{
    public string InsuranceNumber { get; set; } = string.Empty;

    public string ProcedureCode { get; set; } = string.Empty;
}
