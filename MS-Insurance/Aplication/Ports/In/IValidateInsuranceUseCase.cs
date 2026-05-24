using Domain.Enums;

namespace Aplication.Ports.In;

public interface IValidateInsuranceUseCase
{
    Task<ValidationStatus> ValidateProcedureAsync(string insuranceNumber, string procedureCode);
}
