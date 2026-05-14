namespace Aplication.Ports.Out;

public interface IWeatherMonitorPort
{
    /// <summary>
    /// Synchronously queries MS-WeatherMonitor for wind speed along the route.
    /// </summary>
    Task<double> GetWindSpeedAsync(string originZone, string destinationZone);
}