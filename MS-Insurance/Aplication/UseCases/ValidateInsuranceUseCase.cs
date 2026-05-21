using Aplication.Ports.In;
using Aplication.Ports.Out;
using Domain.Enums;
using Domain.Services;

namespace Aplication.UseCases;

public class ValidateInsuranceUseCase : IValidateInsuranceUseCase
{
    private readonly IInsuranceRepositoryPort _repository;
    private readonly InsuranceDomainService _domainService;

    public ValidateInsuranceUseCase(
        IInsuranceRepositoryPort repository,
        InsuranceDomainService domainService)
    {
        _repository = repository;
        _domainService = domainService;
    }

    public async Task<ValidationStatus> ValidateProcedureAsync(
        string insuranceNumber,
        string procedureCode)
    {
        var policy = await _repository.GetByInsuranceNumberAsync(insuranceNumber);
        return _domainService.ValidateCoverage(policy, procedureCode);
    }
}
