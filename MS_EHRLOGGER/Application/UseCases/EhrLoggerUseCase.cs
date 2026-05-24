using MsEhrLogger.Application.Ports.In;
using MsEhrLogger.Application.Ports.Out;
using MsEhrLogger.Domain.Enums;
using MsEhrLogger.Domain.Models;
using MsEhrLogger.Domain.Services;
using MsEhrLogger.Infrastructure.Dtos;
using Prometheus;

namespace MsEhrLogger.Application.UseCases;

/// <summary>
/// Caso de uso principal del MS-EHRLogger.
/// Espejo exacto de AppointmentUseCase.cs del MS-AgendaHub.
///
/// Orquesta la lógica de dominio y coordina los puertos de salida.
///
/// SOLID:
///   SRP — solo orquesta; las reglas de negocio viven en EhrRecord y EhrRecordService.
///   DIP — depende solo de interfaces (puertos), nunca de MongoDB o RabbitMQ.
///
/// Patrones GoF:
///   Strategy  — EhrRecordService selecciona el procesamiento según EventType.
///   Observer  — IEhrEventPublisherPort notifica tras persistir.
///   Builder   — EhrRecordService usa EhrRecordBuilder internamente.
/// </summary>
public class EhrLoggerUseCase : IEhrLoggerUseCasePort
{
    private readonly IEhrRecordRepositoryPort _repository;
    private readonly IEhrEventPublisherPort   _eventPublisher;
    private readonly EhrRecordService         _domainService;
    private readonly ILogger<EhrLoggerUseCase> _logger;

    // Prometheus counters
    private static readonly Counter RecordsStoredCounter = Metrics
        .CreateCounter("ehr_records_stored_total",
            "Registros EHR almacenados exitosamente",
            new CounterConfiguration { LabelNames = new[] { "specialty" } });

    private static readonly Counter ConsumerErrorsCounter = Metrics
        .CreateCounter("ehr_consumer_errors_total",
            "Errores en el consumidor RabbitMQ",
            new CounterConfiguration { LabelNames = new[] { "error_type" } });

    private static readonly Counter DuplicatesCounter = Metrics
        .CreateCounter("ehr_consumer_duplicates_total",
            "Mensajes duplicados detectados (idempotencia)");

    public EhrLoggerUseCase(
        IEhrRecordRepositoryPort   repository,
        IEhrEventPublisherPort     eventPublisher,
        EhrRecordService           domainService,
        ILogger<EhrLoggerUseCase>  logger)
    {
        _repository     = repository;
        _eventPublisher = eventPublisher;
        _domainService  = domainService;
        _logger         = logger;
    }

    // ── Regla de Negocio #3 ────────────────────────────────────────────────
    // El proceso de agenda NO debe esperar la respuesta del historial.
    // Este método es invocado por el consumidor RabbitMQ de forma asíncrona.
    // ──────────────────────────────────────────────────────────────────────
    public async Task<EhrRecord> ProcessAppointmentConfirmedAsync(AppointmentConfirmedEvent ehrEvent)
    {
        _logger.LogInformation(
            "[EHR] Procesando evento AppointmentId={AppointmentId} CorrelationId={CorrelationId}",
            ehrEvent.AppointmentId, ehrEvent.CorrelationId);

        // Idempotencia: si ya existe un registro STORED para esta cita, ignorar
        var existing = await _repository.FindByAppointmentIdAsync(ehrEvent.AppointmentId);
        if (existing is { Status: EhrRecordStatus.Stored })
        {
            _logger.LogWarning("[EHR] Evento duplicado ignorado AppointmentId={AppointmentId}",
                ehrEvent.AppointmentId);
            DuplicatesCounter.Inc();
            return existing;
        }

        // Construir el registro usando el servicio de dominio (Builder + Factory Method)
        var summary = new AppointmentSummary
        {
            ScheduledAt        = ehrEvent.ScheduledAt,
            ConsultationRoom   = ehrEvent.ConsultationRoom,
            InsuranceCode      = ehrEvent.InsuranceCode,
            ProcedureCode      = ehrEvent.ProcedureCode,
            Observations       = ehrEvent.Observations,
            InsuranceValidated = ehrEvent.InsuranceValidated
        };

        var record = _domainService.BuildPendingRecord(
            ehrEvent.AppointmentId,
            ehrEvent.PatientId,
            ehrEvent.DoctorId,
            ehrEvent.Specialty,
            summary,
            ehrEvent.CorrelationId,
            ehrEvent.SourceService);

        // Transición de estado en el dominio
        record.MarkAsStored();

        // Persistir en MongoDB
        var storedRecord = await _repository.SaveAsync(record);

        // Notificar observadores — Patrón Observer
        await _eventPublisher.PublishRecordStoredAsync(storedRecord);

        RecordsStoredCounter.WithLabels(storedRecord.Specialty).Inc();

        _logger.LogInformation(
            "[EHR] Registro almacenado Id={Id} AppointmentId={AppointmentId}",
            storedRecord.Id, storedRecord.AppointmentId);

        return storedRecord;
    }

    public async Task<EhrRecord?> GetRecordByIdAsync(string id) =>
        await _repository.FindByIdAsync(id);

    public async Task<EhrRecord?> GetRecordByAppointmentIdAsync(string appointmentId) =>
        await _repository.FindByAppointmentIdAsync(appointmentId);

    public async Task<IEnumerable<EhrRecord>> GetRecordsByPatientAsync(string patientId) =>
        await _repository.FindByPatientIdAsync(patientId);

    public async Task<IEnumerable<EhrRecord>> GetRecordsByStatusAsync(EhrRecordStatus status) =>
        await _repository.FindByStatusAsync(status);

    public async Task<IEnumerable<EhrRecord>> GetAllRecordsAsync(int page, int size) =>
        await _repository.FindAllAsync(page, size);
}
