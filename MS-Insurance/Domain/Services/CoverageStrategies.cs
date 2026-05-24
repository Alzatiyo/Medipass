namespace Domain.Services;

public class BasicPlanCoverageStrategy : ICoverageStrategy
{
    private static readonly HashSet<string> CoveredProcedures = new(StringComparer.OrdinalIgnoreCase)
    {
        "PROC-BASIC",
        "PROC-CONSULTATION",
        "PROC-001"
    };

    public bool IsProcedureCovered(string procedureCode)
    {
        if (string.IsNullOrWhiteSpace(procedureCode)) return false;
        return CoveredProcedures.Contains(procedureCode);
    }

    public IEnumerable<string> GetCoveredProcedures()
    {
        return CoveredProcedures;
    }
}

public class PremiumPlanCoverageStrategy : ICoverageStrategy
{
    private static readonly HashSet<string> CoveredProcedures = new(StringComparer.OrdinalIgnoreCase)
    {
        "PROC-BASIC",
        "PROC-CONSULTATION",
        "PROC-CARDIOLOGY",
        "PROC-MRI",
        "PROC-001",
        "PROC-002"
    };

    public bool IsProcedureCovered(string procedureCode)
    {
        if (string.IsNullOrWhiteSpace(procedureCode)) return false;
        return CoveredProcedures.Contains(procedureCode);
    }

    public IEnumerable<string> GetCoveredProcedures()
    {
        return CoveredProcedures;
    }
}

public class OncologyPlanCoverageStrategy : ICoverageStrategy
{
    private static readonly HashSet<string> CoveredProcedures = new(StringComparer.OrdinalIgnoreCase)
    {
        "PROC-BASIC",
        "PROC-CONSULTATION",
        "PROC-ONCOLOGY",
        "PROC-CHEMOTHERAPY",
        "PROC-001",
        "PROC-003"
    };

    public bool IsProcedureCovered(string procedureCode)
    {
        if (string.IsNullOrWhiteSpace(procedureCode)) return false;
        return CoveredProcedures.Contains(procedureCode);
    }

    public IEnumerable<string> GetCoveredProcedures()
    {
        return CoveredProcedures;
    }
}
