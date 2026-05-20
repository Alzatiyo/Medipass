using Aplication.Ports.Out;
using Domain.Enums;

namespace Infrastructure.Adapters.Rest;

public class InsuranceServiceAdapter
    : IInsuranceServicePort
{
    public async Task<InsuranceStatus> ValidateProcedureAsync(
        string insuranceNumber,
        string procedureCode)
    {
        await Task.Delay(500);

        if (procedureCode == "NOT_COVERED")
        {
            return InsuranceStatus.ProcedureNotCovered;
        }

        return InsuranceStatus.Approved;
    }
}