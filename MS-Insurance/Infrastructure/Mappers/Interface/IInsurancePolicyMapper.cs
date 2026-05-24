using Domain.Models;
using Infrastructure.Adapters.Persistence;

namespace Infrastructure.Mappers.Interface;

public interface IInsurancePolicyMapper
{
    InsurancePolicy? ToDomain(InsurancePolicyEntity? entity);

    InsurancePolicyEntity? ToEntity(InsurancePolicy? domainObject);
}
