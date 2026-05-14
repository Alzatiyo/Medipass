using Domain.Enums;
using Domain.Models;

namespace Domain.Builders;

public class FlightPlanBuilder
{
    private readonly FlightPlan _plan = new();

    public FlightPlanBuilder WithId(Guid id) { _plan.Id = id; return this; }
    public FlightPlanBuilder WithDroneId(string droneId) { _plan.DroneId = droneId; return this; }
    public FlightPlanBuilder WithOriginZone(string origin) { _plan.OriginZone = origin; return this; }
    public FlightPlanBuilder WithDestinationZone(string destination) { _plan.DestinationZone = destination; return this; }
    public FlightPlanBuilder WithAltitude(double altitude) { _plan.AltitudeMeters = altitude; return this; }
    public FlightPlanBuilder WithZoneType(DroneZoneType zoneType) { _plan.ZoneType = zoneType; return this; }
    public FlightPlanBuilder WithStatus(FlightPlanStatus status) { _plan.Status = status; return this; }
    public FlightPlanBuilder WithCreatedAt(DateTime createdAt) { _plan.CreatedAt = createdAt; return this; }
    public FlightPlanBuilder WithDeliveryProof(string imageUrl) { _plan.DeliveryProofImageUrl = imageUrl; return this; }

    public FlightPlan Build() => _plan;
}