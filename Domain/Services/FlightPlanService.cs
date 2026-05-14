using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;

namespace Domain.Services;

public class FlightPlanService
{
    private const double MaxWindSpeedKmh = 50.0;

    private static readonly Dictionary<DroneZoneType, double> MinAltitudeByZone = new()
    {
        { DroneZoneType.Residential, 50.0 },
        { DroneZoneType.Commercial,  30.0 },
        { DroneZoneType.Industrial,  20.0 },
        { DroneZoneType.Restricted, 100.0 },
        { DroneZoneType.NoFlyZone,  200.0 }
    };

    public void AuthorizeFlightPlan(FlightPlan plan, double windSpeedKmh, bool isNoFlyZoneViolation)
    {
        if (windSpeedKmh >= MaxWindSpeedKmh)
            throw new DomainException(
                $"Flight plan rejected: wind speed {windSpeedKmh} km/h exceeds maximum allowed {MaxWindSpeedKmh} km/h.");

        if (isNoFlyZoneViolation)
            throw new DomainException(
                "Flight plan rejected: route passes through a restricted No-Fly Zone.");

        double minAltitude = MinAltitudeByZone.GetValueOrDefault(plan.ZoneType, 50.0);
        if (plan.AltitudeMeters < minAltitude)
            throw new DomainException(
                $"Flight plan rejected: altitude {plan.AltitudeMeters}m is below the minimum {minAltitude}m for zone type {plan.ZoneType}.");

        plan.WindSpeedKmh = windSpeedKmh;
        plan.IsNoFlyZoneViolation = false;
        plan.Status = FlightPlanStatus.Authorized;
        plan.AuthorizedAt = DateTime.UtcNow;
    }

    public void RejectFlightPlan(FlightPlan plan, string reason)
    {
        plan.Status = FlightPlanStatus.Rejected;
        plan.RejectionReason = reason;
    }

    public void MarkAsDelivered(FlightPlan plan, string proofImageUrl)
    {
        if (plan.Status != FlightPlanStatus.InFlight)
            throw new DomainException("Only in-flight plans can be marked as delivered.");

        if (string.IsNullOrWhiteSpace(proofImageUrl))
            throw new DomainException("Delivery proof image URL is required.");

        plan.Status = FlightPlanStatus.Delivered;
        plan.DeliveredAt = DateTime.UtcNow;
        plan.DeliveryProofImageUrl = proofImageUrl;
    }
}