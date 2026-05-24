using Domain.Enums;

namespace Aplication.Ports.Out;

public interface IInsuranceServicePort
{
    Task<InsuranceStatus> ValidateProcedureAsync(
        string insuranceNumber,
        string procedureCode);
}