using Domain.Models;

namespace Aplication.Ports.Out;

public interface IFlightPlanRepositoryPort
{
    Task<FlightPlan> SaveAsync(FlightPlan plan);
    Task<FlightPlan?> FindByIdAsync(Guid id);
    Task<IEnumerable<FlightPlan>> FindAllAsync();
    Task<FlightPlan> UpdateAsync(FlightPlan plan);
}