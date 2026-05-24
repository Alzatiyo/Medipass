using Domain.Enums;
using Domain.Models;
using Infrastructure.Adapters.Persistence;
using Infrastructure.Mappers.Interface;

namespace Infrastructure.Mappers;

public class InsurancePolicyMapper : IInsurancePolicyMapper
{
    public InsurancePolicy? ToDomain(InsurancePolicyEntity? entity)
    {
        if (entity == null) return null;

        if (!Enum.TryParse<PlanType>(entity.Plan, true, out var planType))
        {
            planType = PlanType.Basic;
        }

        return new InsurancePolicy
        {
            Id = entity.Id,
            InsuranceNumber = entity.InsuranceNumber,
            PatientName = entity.PatientName,
            Plan = planType,
            IsActive = entity.IsActive,
            ExpirationDate = entity.ExpirationDate
        };
    }

    public InsurancePolicyEntity? ToEntity(InsurancePolicy? domainObject)
    {
        if (domainObject == null) return null;

        return new InsurancePolicyEntity
        {
            Id = domainObject.Id,
            InsuranceNumber = domainObject.InsuranceNumber,
            PatientName = domainObject.PatientName,
            Plan = domainObject.Plan.ToString(),
            IsActive = domainObject.IsActive,
            ExpirationDate = domainObject.ExpirationDate
        };
    }
}
