namespace Aplication.Ports.Out;

public interface INoFlyZonePort
{
    /// <summary>
    /// Synchronously validates whether a route violates any No-Fly Zone.
    /// </summary>
    Task<bool> IsRouteRestrictedAsync(string originZone, string destinationZone, double altitudeMeters);
}