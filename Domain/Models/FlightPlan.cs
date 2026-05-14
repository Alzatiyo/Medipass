using Domain.Enums;

namespace Domain.Models;

public class FlightPlan
{
    public Guid Id { get; set; }
    public string DroneId { get; set; } = string.Empty;
    public string OriginZone { get; set; } = string.Empty;
    public string DestinationZone { get; set; } = string.Empty;
    public double AltitudeMeters { get; set; }
    public DroneZoneType ZoneType { get; set; }
    public FlightPlanStatus Status { get; set; }
    public double WindSpeedKmh { get; set; }
    public bool IsNoFlyZoneViolation { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AuthorizedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? DeliveryProofImageUrl { get; set; }
}