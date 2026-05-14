using Aplication.Ports.Out;
using System.Text.Json;

namespace Infrastructure.Adapters.Rest;

public class NoFlyZoneAdapter : INoFlyZonePort
{
    private readonly HttpClient _httpClient;

    public NoFlyZoneAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsRouteRestrictedAsync(
        string originZone, string destinationZone, double altitudeMeters)
    {
        var response = await _httpClient.GetAsync(
            $"/api/noflyzone/validate?origin={Uri.EscapeDataString(originZone)}" +
            $"&destination={Uri.EscapeDataString(destinationZone)}" +
            $"&altitude={altitudeMeters}");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        return doc.RootElement.GetProperty("isRestricted").GetBoolean();
    }
}