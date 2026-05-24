namespace Domain.Services;

public interface ICoverageStrategy
{
    bool IsProcedureCovered(string procedureCode);

    IEnumerable<string> GetCoveredProcedures();
}
