using Prometheus;

namespace MsEhrLogger.Infrastructure.Observability;

public static class EhrMetrics
{
    public static readonly Counter RecordsSaved =
        Metrics.CreateCounter(
            "medipass_ehr_records_saved_total",
            "Total de registros EHR persistidos en MongoDB.");

    public static readonly Counter DuplicatesDiscarded =
        Metrics.CreateCounter(
            "medipass_ehr_duplicates_discarded_total",
            "Total de mensajes duplicados detectados y descartados (idempotencia).");

    public static readonly Counter DeadLetterMessages =
        Metrics.CreateCounter(
            "medipass_ehr_dead_letter_total",
            "Total de mensajes enviados a la DLQ tras agotar reintentos.");

    public static readonly Histogram ProcessingDuration =
        Metrics.CreateHistogram(
            "medipass_ehr_processing_duration_seconds",
            "Duración del procesamiento de un evento AppointmentConfirmed.",
            new HistogramConfiguration
            {
                Buckets = Histogram.LinearBuckets(start: 0.005, width: 0.025, count: 20)
            });

    public static readonly Gauge ConsumerActive =
        Metrics.CreateGauge(
            "medipass_ehr_consumer_active",
            "Estado del consumidor de RabbitMQ: 1 = activo, 0 = caído.");
}
