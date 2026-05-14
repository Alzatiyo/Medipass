using Domain.Enums;

namespace Infrastructure.Adapters.Persistence;

public class NoFlyZoneEntity
{
    public Guid Id { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public DroneZoneType ZoneType { get; set; }
    public double MinLatitude { get; set; }
    public double MaxLatitude { get; set; }
    public double MinLongitude { get; set; }
    public double MaxLongitude { get; set; }
    public double MinAltitudeMeters { get; set; }
    public bool IsActive { get; set; }
}