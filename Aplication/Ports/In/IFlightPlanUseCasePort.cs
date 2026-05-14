using Domain.Models;

namespace Aplication.Ports.In;

public interface IFlightPlanUseCasePort
{
    Task<FlightPlan> SubmitFlightPlanAsync(FlightPlan plan);
    Task<FlightPlan> GetFlightPlanAsync(Guid id);
    Task<IEnumerable<FlightPlan>> GetAllFlightPlansAsync();
    Task<FlightPlan> RegisterDeliveryAsync(Guid flightPlanId, string proofImageUrl);
    Task<FlightPlan> UpdateFlightStatusAsync(Guid flightPlanId, string newStatus);
}