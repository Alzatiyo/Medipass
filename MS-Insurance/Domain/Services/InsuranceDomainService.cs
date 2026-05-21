using Domain.Enums;
using Domain.Models;

namespace Domain.Services;

public class InsuranceDomainService
{
    public ValidationStatus ValidateCoverage(InsurancePolicy? policy, string procedureCode)
    {
        // Regla 1: Validación de Existencia de la Póliza
        if (policy == null)
        {
            return ValidationStatus.InvalidPolicy;
        }

        // Regla 2: Validación de Estado Activo y Vigencia de la Póliza
        if (!policy.IsActive || policy.ExpirationDate <= DateTime.UtcNow)
        {
            return ValidationStatus.PlanInactive;
        }

        // Regla 3: Validación de Cobertura del Procedimiento según el Plan
        var strategy = CoverageStrategyFactory.GetStrategy(policy.Plan);
        if (!strategy.IsProcedureCovered(procedureCode))
        {
            return ValidationStatus.ProcedureNotCovered;
        }

        return ValidationStatus.Approved;
    }
}
