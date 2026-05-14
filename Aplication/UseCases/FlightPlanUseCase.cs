using Aplication.Ports.In;
using Aplication.Ports.Out;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services;

namespace Aplication.UseCases;

public class FlightPlanUseCase : IFlightPlanUseCasePort
{
    private readonly IFlightPlanRepositoryPort _repository;
    private readonly IWeatherMonitorPort _weatherMonitor;
    private readonly INoFlyZonePort _noFlyZone;
    private readonly IDeliveryProofPort _deliveryProof;
    private readonly FlightPlanService _domainService;

    public FlightPlanUseCase(
        IFlightPlanRepositoryPort repository,
        IWeatherMonitorPort weatherMonitor,
        INoFlyZonePort noFlyZone,
        IDeliveryProofPort deliveryProof,
        FlightPlanService domainService)
    {
        _repository = repository;
        _weatherMonitor = weatherMonitor;
        _noFlyZone = noFlyZone;
        _deliveryProof = deliveryProof;
        _domainService = domainService;
    }

    public async Task<FlightPlan> SubmitFlightPlanAsync(FlightPlan plan)
    {
        plan.Id = Guid.NewGuid();
        plan.CreatedAt = DateTime.UtcNow;
        plan.Status = FlightPlanStatus.Pending;

        // Sync call to MS-WeatherMonitor
        double windSpeed = await _weatherMonitor.GetWindSpeedAsync(plan.OriginZone, plan.DestinationZone);

        // Sync call to validate No-Fly Zones
        bool isRestricted = await _noFlyZone.IsRouteRestrictedAsync(
            plan.OriginZone, plan.DestinationZone, plan.AltitudeMeters);

        try
        {
            _domainService.AuthorizeFlightPlan(plan, windSpeed, isRestricted);
        }
        catch (DomainException ex)
        {
            _domainService.RejectFlightPlan(plan, ex.Message);
        }

        return await _repository.SaveAsync(plan);
    }

    public async Task<FlightPlan> GetFlightPlanAsync(Guid id)
    {
        var plan = await _repository.FindByIdAsync(id);
        if (plan is null)
            throw new DomainException($"Flight plan with ID {id} not found.");
        return plan;
    }

    public async Task<IEnumerable<FlightPlan>> GetAllFlightPlansAsync()
        => await _repository.FindAllAsync();

    public async Task<FlightPlan> RegisterDeliveryAsync(Guid flightPlanId, string proofImageUrl)
    {
        var plan = await GetFlightPlanAsync(flightPlanId);

        _domainService.MarkAsDelivered(plan, proofImageUrl);
        var updated = await _repository.UpdateAsync(plan);

        // Async publish to MS-DeliveryProof via broker (fire-and-forget style)
        _ = _deliveryProof.PublishDeliveryProofAsync(
            plan.Id, plan.DroneId, proofImageUrl, plan.DeliveredAt!.Value);

        return updated;
    }

    public async Task<FlightPlan> UpdateFlightStatusAsync(Guid flightPlanId, string newStatus)
    {
        var plan = await GetFlightPlanAsync(flightPlanId);

        if (!Enum.TryParse<FlightPlanStatus>(newStatus, ignoreCase: true, out var status))
            throw new DomainException($"Invalid status: {newStatus}");

        plan.Status = status;
        return await _repository.UpdateAsync(plan);
    }
}