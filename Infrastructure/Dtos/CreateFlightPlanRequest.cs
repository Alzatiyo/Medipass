using Domain.Enums;

namespace Infrastructure.Dtos;

public class CreateFlightPlanRequest
{
    public string DroneId { get; set; } = string.Empty;
    public string OriginZone { get; set; } = string.Empty;
    public string DestinationZone { get; set; } = string.Empty;
    public double AltitudeMeters { get; set; }
    public DroneZoneType ZoneType { get; set; }
}