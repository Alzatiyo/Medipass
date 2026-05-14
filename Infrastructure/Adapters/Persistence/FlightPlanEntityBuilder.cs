using Domain.Enums;

namespace Infrastructure.Adapters.Persistence;

public class FlightPlanEntityBuilder
{
    private readonly FlightPlanEntity _entity = new();

    public FlightPlanEntityBuilder WithId(Guid id) { _entity.Id = id; return this; }
    public FlightPlanEntityBuilder WithDroneId(string droneId) { _entity.DroneId = droneId; return this; }
    public FlightPlanEntityBuilder WithOriginZone(string origin) { _entity.OriginZone = origin; return this; }
    public FlightPlanEntityBuilder WithDestinationZone(string dest) { _entity.DestinationZone = dest; return this; }
    public FlightPlanEntityBuilder WithAltitude(double altitude) { _entity.AltitudeMeters = altitude; return this; }
    public FlightPlanEntityBuilder WithZoneType(DroneZoneType zoneType) { _entity.ZoneType = zoneType; return this; }
    public FlightPlanEntityBuilder WithStatus(FlightPlanStatus status) { _entity.Status = status; return this; }
    public FlightPlanEntityBuilder WithWindSpeed(double wind) { _entity.WindSpeedKmh = wind; return this; }
    public FlightPlanEntityBuilder WithNoFlyViolation(bool violation) { _entity.IsNoFlyZoneViolation = violation; return this; }
    public FlightPlanEntityBuilder WithRejectionReason(string? reason) { _entity.RejectionReason = reason; return this; }
    public FlightPlanEntityBuilder WithCreatedAt(DateTime dt) { _entity.CreatedAt = dt; return this; }
    public FlightPlanEntityBuilder WithAuthorizedAt(DateTime? dt) { _entity.AuthorizedAt = dt; return this; }
    public FlightPlanEntityBuilder WithDeliveredAt(DateTime? dt) { _entity.DeliveredAt = dt; return this; }
    public FlightPlanEntityBuilder WithDeliveryProof(string? url) { _entity.DeliveryProofImageUrl = url; return this; }

    public FlightPlanEntity Build() => _entity;
}