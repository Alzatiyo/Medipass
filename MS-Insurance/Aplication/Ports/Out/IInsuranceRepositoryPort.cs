using Domain.Models;

namespace Aplication.Ports.Out;

public interface IInsuranceRepositoryPort
{
    Task<InsurancePolicy?> GetByInsuranceNumberAsync(string insuranceNumber);

    Task SaveAsync(InsurancePolicy policy);
}
