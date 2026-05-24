using System.Text;
using MsEhrLogger.Application.Ports.In;
using MsEhrLogger.Domain.Exceptions;
using MsEhrLogger.Infrastructure.Dtos;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MsEhrLogger.Infrastructure.Adapters.Rest;

/// <summary>
/// Consumidor asíncrono de RabbitMQ.
/// Escucha la cola publicada por EhrEventPublisherAdapter.cs del MS-AgendaHub.
///
/// REGLA DE NEGOCIO #3:
/// "Tras confirmar la cita, el resumen debe enviarse al Historial Clínico Digital
/// de forma asíncrona; el proceso de agenda NO debe esperar al historial."
///
/// Nombre: AppointmentConsumerAdapter — simetría con los adapters del MS-AgendaHub.
/// </summary>
public class AppointmentConsumerAdapter : BackgroundService
{
    private const string MainExchange   = "medipass.events";
    private const string EhrQueue       = "medipass.ehr.appointments";
    private const string RoutingKey     = "appointment.confirmed";
    private const string DlxExchange    = "medipass.dlx";
    private const string DlqQueue       = "medipass.ehr.appointments.dlq";
    private const string DlqRoutingKey  = "ehr.dlq";

    private readonly IEhrLoggerUseCasePort _useCase;
    private readonly IConnection           _connection;
    private readonly IModel                _channel;
    private readonly ILogger<AppointmentConsumerAdapter> _logger;

    public AppointmentConsumerAdapter(
        IEhrLoggerUseCasePort                    useCase,
        IConnection                              connection,
        ILogger<AppointmentConsumerAdapter>      logger)
    {
        _useCase    = useCase;
        _connection = connection;
        _channel    = _connection.CreateModel();
        _logger     = logger;

        DeclareTopology();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnMessageReceivedAsync;

        _channel.BasicConsume(
            queue:       EhrQueue,
            autoAck:     false,
            consumer:    consumer);

        _logger.LogInformation(
            "[EHR-Consumer] Escuchando cola {Queue} en exchange {Exchange}",
            EhrQueue, MainExchange);

        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs args)
    {
        var body    = args.Body.ToArray();
        var payload = Encoding.UTF8.GetString(body);

        _logger.LogInformation(
            "[EHR-Consumer] Mensaje recibido RoutingKey={Key}", args.RoutingKey);

        try
        {
            var ehrEvent = JsonConvert.DeserializeObject<AppointmentConfirmedEvent>(payload);
            if (ehrEvent is null)
                throw new InvalidEhrRecordException("El payload del evento es nulo o inválido.");

            _logger.LogInformation(
                "[EHR-Consumer] Procesando AppointmentId={Id} CorrelationId={Corr}",
                ehrEvent.AppointmentId, ehrEvent.CorrelationId);

            await _useCase.ProcessAppointmentConfirmedAsync(ehrEvent);

            // ACK: mensaje procesado correctamente
            _channel.BasicAck(args.DeliveryTag, multiple: false);
        }
        catch (InvalidEhrRecordException ex)
        {
            // Error de dominio: no se reintenta, NACK directo a DLQ
            _logger.LogError(
                "[EHR-Consumer] Error de dominio (no reintentable): {Msg}", ex.Message);
            _channel.BasicNack(args.DeliveryTag, multiple: false, requeue: false);
        }
        catch (Exception ex)
        {
            // Error transitorio: RabbitMQ reintentará según la política TTL/DLQ
            _logger.LogError(ex,
                "[EHR-Consumer] Error transitorio procesando mensaje");
            _channel.BasicNack(args.DeliveryTag, multiple: false, requeue: true);
        }
    }

    // ── Topología RabbitMQ ─────────────────────────────────────────────────

    private void DeclareTopology()
    {
        // Dead Letter Exchange
        _channel.ExchangeDeclare(DlxExchange, ExchangeType.Direct, durable: true);

        // Dead Letter Queue
        _channel.QueueDeclare(DlqQueue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(DlqQueue, DlxExchange, DlqRoutingKey);

        // Exchange principal
        _channel.ExchangeDeclare(MainExchange, ExchangeType.Topic, durable: true);

        // Cola principal con DLQ configurada
        var queueArgs = new Dictionary<string, object>
        {
            ["x-dead-letter-exchange"]    = DlxExchange,
            ["x-dead-letter-routing-key"] = DlqRoutingKey,
            ["x-message-ttl"]             = 300_000 // 5 minutos
        };

        _channel.QueueDeclare(EhrQueue,
            durable:    true,
            exclusive:  false,
            autoDelete: false,
            arguments:  queueArgs);

        _channel.QueueBind(EhrQueue, MainExchange, RoutingKey);

        // Prefetch para control de flujo
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);
    }

    public override void Dispose()
    {
        _channel?.Close();
        base.Dispose();
    }
}
