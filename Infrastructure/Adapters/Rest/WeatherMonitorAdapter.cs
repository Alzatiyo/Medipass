using Aplication.Ports.Out;
using System.Text.Json;

namespace Infrastructure.Adapters.Rest;

public class WeatherMonitorAdapter : IWeatherMonitorPort
{
    private readonly HttpClient _httpClient;

    public WeatherMonitorAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<double> GetWindSpeedAsync(string originZone, string destinationZone)
    {
        var response = await _httpClient.GetAsync(
            $"/api/weather/wind-speed?origin={Uri.EscapeDataString(originZone)}&destination={Uri.EscapeDataString(destinationZone)}");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        return doc.RootElement.GetProperty("windSpeedKmh").GetDouble();
    }
}