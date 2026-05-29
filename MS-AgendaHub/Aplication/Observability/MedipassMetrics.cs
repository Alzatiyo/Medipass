using Prometheus;

namespace Aplication.Observability;

public static class MedipassMetrics
{
    public static readonly Counter AppointmentsConfirmed =
        Metrics.CreateCounter(
            "medipass_appointments_confirmed_total",
            "Total de citas médicas confirmadas.",
            new CounterConfiguration
            {
                LabelNames = new[] { "specialty" }
            });

    public static readonly Counter AppointmentsRejected =
        Metrics.CreateCounter(
            "medipass_appointments_rejected_total",
            "Total de intentos de cita rechazados.",
            new CounterConfiguration
            {
                LabelNames = new[] { "reason" }
            });

    public static readonly Counter InsuranceRejections =
        Metrics.CreateCounter(
            "medipass_insurance_rejections_total",
            "Total de rechazos por procedimiento no cubierto por el seguro.");

    public static readonly Histogram InsuranceCallDuration =
        Metrics.CreateHistogram(
            "medipass_insurance_call_duration_seconds",
            "Latencia de la validación síncrona con MS-Insurance.",
            new HistogramConfiguration
            {
                Buckets = Histogram.LinearBuckets(start: 0.01, width: 0.05, count: 20)
            });

    public static readonly Counter EhrEventsPublished =
        Metrics.CreateCounter(
            "medipass_ehr_events_published_total",
            "Total de eventos AppointmentConfirmed enviados a RabbitMQ.");

    public static readonly Counter EhrPublishFailures =
        Metrics.CreateCounter(
            "medipass_ehr_publish_failures_total",
            "Total de fallos al publicar eventos al broker de mensajería.");

    public static readonly Counter DoubleBookingBlocked =
        Metrics.CreateCounter(
            "medipass_double_booking_blocked_total",
            "Total de intentos de double-booking detectados y bloqueados.");
}
