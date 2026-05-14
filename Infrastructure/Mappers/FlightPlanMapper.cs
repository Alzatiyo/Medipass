using Domain.Builders;
using Domain.Models;
using Infrastructure.Adapters.Persistence;
using Infrastructure.Mappers.Interface;

namespace Infrastructure.Mappers;

public class FlightPlanMapper : IFlightPlanMapper
{
    public FlightPlan ToDomain(FlightPlanEntity e) =>
        new FlightPlanBuilder()
            .WithId(e.Id)
            .WithDroneId(e.DroneId)
            .WithOriginZone(e.OriginZone)
            .WithDestinationZone(e.DestinationZone)
            .WithAltitude(e.AltitudeMeters)
            .WithZoneType(e.ZoneType)
            .WithStatus(e.Status)
            .WithCreatedAt(e.CreatedAt)
            .WithDeliveryProof(e.DeliveryProofImageUrl ?? string.Empty)
            .Build();

    public FlightPlanEntity ToEntity(FlightPlan d) =>
        new FlightPlanEntityBuilder()
            .WithId(d.Id)
            .WithDroneId(d.DroneId)
            .WithOriginZone(d.OriginZone)
            .WithDestinationZone(d.DestinationZone)
            .WithAltitude(d.AltitudeMeters)
            .WithZoneType(d.ZoneType)
            .WithStatus(d.Status)
            .WithWindSpeed(d.WindSpeedKmh)
            .WithNoFlyViolation(d.IsNoFlyZoneViolation)
            .WithRejectionReason(d.RejectionReason)
            .WithCreatedAt(d.CreatedAt)
            .WithAuthorizedAt(d.AuthorizedAt)
            .WithDeliveredAt(d.DeliveredAt)
            .WithDeliveryProof(d.DeliveryProofImageUrl)
            .Build();
}