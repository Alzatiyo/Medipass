using Aplication.Ports.Out;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Adapters.Rest;

/// <summary>
/// Adapter that publishes delivery proof events to MS-DeliveryProof.
/// In production this would use a message broker (RabbitMQ / Kafka).
/// The HTTP call is fire-and-forget from the use case perspective.
/// </summary>
public class DeliveryProofAdapter : IDeliveryProofPort
{
    private readonly HttpClient _httpClient;

    public DeliveryProofAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task PublishDeliveryProofAsync(
        Guid flightPlanId, string droneId, string imageUrl, DateTime deliveredAt)
    {
        var payload = new
        {
            flightPlanId,
            droneId,
            imageUrl,
            deliveredAt
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        // Best-effort: we do not await a meaningful result
        await _httpClient.PostAsync("/api/delivery-proof", content);
    }
}