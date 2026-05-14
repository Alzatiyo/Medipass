using Domain.Models;
using Infrastructure.Adapters.Persistence;

namespace Infrastructure.Mappers.Interface;

public interface IFlightPlanMapper
{
    FlightPlan ToDomain(FlightPlanEntity entity);
    FlightPlanEntity ToEntity(FlightPlan domain);
}