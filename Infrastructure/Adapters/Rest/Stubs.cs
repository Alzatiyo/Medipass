using Aplication.Ports.Out;

namespace Infrastructure.Adapters.Rest;

// Devuelve 30 km/h ? siempre bajo el límite de 50, plan autorizado
public class WeatherMonitorStub : IWeatherMonitorPort
{
    public Task<double> GetWindSpeedAsync(string origin, string destination)
        => Task.FromResult(30.0);
}

// Devuelve false ? ruta no restringida, plan autorizado
public class NoFlyZoneStub : INoFlyZonePort
{
    public Task<bool> IsRouteRestrictedAsync(string origin, string destination, double altitude)
        => Task.FromResult(false);
}

// No hace nada, simula el broker asíncrono
public class DeliveryProofStub : IDeliveryProofPort
{
    public Task PublishDeliveryProofAsync(Guid flightPlanId, string droneId, string imageUrl, DateTime deliveredAt)
        => Task.CompletedTask;
}