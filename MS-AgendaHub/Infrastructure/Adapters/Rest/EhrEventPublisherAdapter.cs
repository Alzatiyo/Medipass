using System.Text;
using Application.Ports.Out;
using Domain.Models;
using Infrastructure.Dtos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure.Adapters.Rest;

/// <summary>
/// Adaptador de salida que publica eventos de cita confirmada
/// hacia RabbitMQ para que MS-EHRLogger los consuma de forma asíncrona.
///
/// Implementa IEhrEventPublisherPort (Puerto de Salida — DIP).
///
/// REGLA DE NEGOCIO #3:
/// "El proceso de agenda NO debe esperar a que el historial responda."
/// Por eso cualquier excepción se loggea pero NO se propaga:
/// el agendamiento ya fue confirmado y no se puede revertir por un fallo del EHR.
///
/// Patrones aplicados:
///   Adapter  — adapta RabbitMQ.Client al contrato IEhrEventPublisherPort.
///   Observer — notifica al MS-EHRLogger sin acoplamiento directo.
/// </summary>
public class EhrEventPublisherAdapter : IEhrEventPublisherPort, IDisposable
{
    // ── Topología RabbitMQ — debe coincidir con AppointmentConsumerAdapter.cs del MS-EHRLogger
    private const string Exchange   = "medipass.events";
    private const string RoutingKey = "appointment.confirmed";

    private readonly IModel                            _channel;
    private readonly ILogger<EhrEventPublisherAdapter> _logger;

    public EhrEventPublisherAdapter(
        IConnection                            connection,
        ILogger<EhrEventPublisherAdapter>      logger)
    {
        _logger  = logger;
        _channel = connection.CreateModel();

        // Declarar el exchange idempotentemente.
        // Si MS-EHRLogger ya lo declaró, esta llamada no falla.
        _channel.ExchangeDeclare(
            exchange: Exchange,
            type:     ExchangeType.Topic,
            durable:  true,
            autoDelete: false);
    }

    /// <summary>
    /// Serializa la cita como AppointmentConfirmedEvent y la publica
    /// en el exchange de RabbitMQ con entrega persistente (DeliveryMode = 2).
    /// </summary>
    public async Task PublishAppointmentAsync(Appointment appointment)
    {
        try
        {
            var ehrEvent = BuildEvent(appointment);

            var json  = JsonConvert.SerializeObject(ehrEvent, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            });
            var body  = Encoding.UTF8.GetBytes(json);

            // Propiedades del mensaje
            var props = _channel.CreateBasicProperties();
            props.ContentType  = "application/json";
            props.DeliveryMode = 2;                          // Persistente: sobrevive reinicios del broker
            props.MessageId    = ehrEvent.EventId;           // Para idempotencia en MS-EHRLogger
            props.CorrelationId = ehrEvent.CorrelationId;    // Para trazabilidad en Jaeger

            _channel.BasicPublish(
                exchange:        Exchange,
                routingKey:      RoutingKey,
                mandatory:       false,
                basicProperties: props,
                body:            body);

            _logger.LogInformation(
                "[AgendaHub→EHR] Evento publicado. AppointmentId={AppointmentId} " +
                "CorrelationId={CorrelationId} RoutingKey={RoutingKey}",
                appointment.Id,
                appointment.CorrelationId,
                RoutingKey);
        }
        catch (Exception ex)
        {
            // ⚠️  NO relanzar: el agendamiento ya fue confirmado.
            // El MS-EHRLogger tiene su propia DLQ para mensajes perdidos.
            _logger.LogError(ex,
                "[AgendaHub→EHR] Error publicando evento. AppointmentId={AppointmentId}. " +
                "El historial clínico se actualizará en el próximo reintento del broker.",
                appointment.Id);
        }

        // Mantiene la firma async para compatibilidad con IEhrEventPublisherPort
        await Task.CompletedTask;
    }

    // ── Construcción del evento ────────────────────────────────────────────

    private static AppointmentConfirmedEvent BuildEvent(Appointment appointment) =>
        new()
        {
            EventId            = Guid.NewGuid().ToString(),
            AppointmentId      = appointment.Id,
            PatientId          = appointment.Patient.Id,
            DoctorId           = appointment.Doctor.Id,
            Specialty          = appointment.Doctor.Specialty.ToString(),
            ScheduledAt        = appointment.ScheduledAt,
            ConsultationRoom   = appointment.ConsultationRoom   ?? string.Empty,
            InsuranceCode      = appointment.InsuranceCode      ?? string.Empty,
            ProcedureCode      = appointment.ProcedureCode      ?? string.Empty,
            InsuranceValidated = appointment.InsuranceValidated,
            Observations       = appointment.Observations       ?? string.Empty,
            CorrelationId      = appointment.CorrelationId      ?? Guid.NewGuid().ToString(),
            SourceService      = "ms-agendahub",
            PublishedAt        = DateTime.UtcNow
        };

    public void Dispose()
    {
        if (_channel.IsOpen)
            _channel.Close();
    }
}
