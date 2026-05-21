using Domain.Enums;

namespace Domain.Services;

public class CoverageStrategyFactory
{
    public static ICoverageStrategy GetStrategy(PlanType planType)
    {
        return planType switch
        {
            PlanType.Basic => new BasicPlanCoverageStrategy(),
            PlanType.Premium => new PremiumPlanCoverageStrategy(),
            PlanType.Oncology => new OncologyPlanCoverageStrategy(),
            _ => throw new ArgumentOutOfRangeException(nameof(planType), planType, null)
        };
    }
}
