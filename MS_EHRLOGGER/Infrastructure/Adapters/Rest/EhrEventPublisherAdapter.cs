using System.Text;
using MsEhrLogger.Application.Ports.Out;
using MsEhrLogger.Domain.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MsEhrLogger.Infrastructure.Adapters.Rest;

/// <summary>
/// Patrón GoF: ADAPTER + OBSERVER (implementación concreta)
///
/// Publica eventos de auditoría a RabbitMQ tras almacenar un registro EHR.
///
/// El nombre EhrEventPublisherAdapter es deliberado: espeja exactamente
/// el EhrEventPublisherAdapter.cs del MS-AgendaHub para mantener simetría
/// de nomenclatura entre ambos microservicios.
/// </summary>
public class EhrEventPublisherAdapter : IEhrEventPublisherPort, IDisposable
{
    private const string AuditExchange   = "medipass.events";
    private const string RoutingStored   = "ehr.record.stored";
    private const string RoutingFailed   = "ehr.record.failed";

    private readonly IConnection _connection;
    private readonly IModel      _channel;
    private readonly ILogger<EhrEventPublisherAdapter> _logger;

    public EhrEventPublisherAdapter(
        IConnection                        connection,
        ILogger<EhrEventPublisherAdapter>  logger)
    {
        _connection = connection;
        _channel    = _connection.CreateModel();
        _logger     = logger;

        _channel.ExchangeDeclare(
            exchange: AuditExchange,
            type:     ExchangeType.Topic,
            durable:  true);
    }

    public Task PublishRecordStoredAsync(EhrRecord record)
    {
        try
        {
            var message = BuildMessage(record, "STORED", null);
            Publish(RoutingStored, message);
            _logger.LogDebug("[EHR-Publisher] Evento STORED publicado AppointmentId={Id}",
                record.AppointmentId);
        }
        catch (Exception ex)
        {
            // No debe fallar el flujo principal
            _logger.LogWarning("[EHR-Publisher] No se pudo publicar evento STORED: {Msg}",
                ex.Message);
        }
        return Task.CompletedTask;
    }

    public Task PublishRecordFailedAsync(EhrRecord record, string errorReason)
    {
        try
        {
            var message = BuildMessage(record, "FAILED", errorReason);
            Publish(RoutingFailed, message);
            _logger.LogWarning("[EHR-Publisher] Evento FAILED publicado AppointmentId={Id}",
                record.AppointmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError("[EHR-Publisher] No se pudo publicar evento FAILED: {Msg}",
                ex.Message);
        }
        return Task.CompletedTask;
    }

    private void Publish(string routingKey, object message)
    {
        var body  = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        var props = _channel.CreateBasicProperties();
        props.ContentType  = "application/json";
        props.DeliveryMode = 2; // Persistent

        _channel.BasicPublish(
            exchange:   AuditExchange,
            routingKey: routingKey,
            basicProperties: props,
            body:       body);
    }

    private static object BuildMessage(EhrRecord record, string outcome, string? errorReason) =>
        new
        {
            recordId      = record.Id,
            appointmentId = record.AppointmentId,
            patientId     = record.PatientId,
            specialty     = record.Specialty,
            correlationId = record.CorrelationId,
            outcome,
            processedAt   = record.ProcessedAt?.ToString("O"),
            errorReason
        };

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
