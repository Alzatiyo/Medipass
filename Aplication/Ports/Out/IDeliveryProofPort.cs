namespace Aplication.Ports.Out;

public interface IDeliveryProofPort
{
    /// <summary>
    /// Asynchronously dispatches the delivery proof to MS-DeliveryProof via broker.
    /// </summary>
    Task PublishDeliveryProofAsync(Guid flightPlanId, string droneId, string imageUrl, DateTime deliveredAt);
}