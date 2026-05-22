using MsEhrLogger.Domain.Builders;
using MsEhrLogger.Domain.Enums;
using MsEhrLogger.Domain.Exceptions;
using MsEhrLogger.Domain.Models;
using MsEhrLogger.Domain.Services;

namespace MsEhrLogger.Tests.Domain;

/// <summary>
/// Tests unitarios del modelo de dominio EhrRecord.
/// Misma convención de nomenclatura que los tests del MS-AgendaHub.
/// No requieren infraestructura — pure unit tests.
/// </summary>
public class EhrRecordTests
{
    private const string AppointmentId  = "apt-001";
    private const string PatientId      = "pat-001";
    private const string DoctorId       = "doc-001";
    private const string Specialty      = "Oncología";
    private const string CorrelationId  = "corr-abc-123";
    private const string SourceService  = "ms-agendahub";

    // ── Factory Method ─────────────────────────────────────────────────────

    [Fact]
    public void CreateFromAppointmentConfirmed_DebeCrearRegistroValido()
    {
        var record = BuildValidRecord();

        Assert.NotNull(record.Id);
        Assert.Equal(AppointmentId, record.AppointmentId);
        Assert.Equal(EhrRecordStatus.Pending, record.Status);
        Assert.Equal(0, record.RetryCount);
        Assert.True(record.OccurredAt <= DateTime.UtcNow);
    }

    // ── Validaciones de dominio ────────────────────────────────────────────

    [Fact]
    public void CreateFromAppointmentConfirmed_DebefallarSiAppointmentIdEsNulo()
    {
        Assert.Throws<InvalidEhrRecordException>(() =>
            EhrRecord.CreateFromAppointmentConfirmed(
                null!, PatientId, DoctorId, Specialty,
                new AppointmentSummary(), CorrelationId, SourceService));
    }

    [Fact]
    public void CreateFromAppointmentConfirmed_DebeFallarSiCorrelationIdEsNulo()
    {
        Assert.Throws<InvalidEhrRecordException>(() =>
            EhrRecord.CreateFromAppointmentConfirmed(
                AppointmentId, PatientId, DoctorId, Specialty,
                new AppointmentSummary(), null!, SourceService));
    }

    // ── Transiciones de estado ─────────────────────────────────────────────

    [Fact]
    public void MarkAsStored_DebeCambiarEstadoAStored()
    {
        var record = BuildValidRecord();
        record.MarkAsStored();

        Assert.Equal(EhrRecordStatus.Stored, record.Status);
        Assert.NotNull(record.ProcessedAt);
    }

    [Fact]
    public void MarkAsStored_DebeLanzarExcepcionSiYaEstaStored()
    {
        var record = BuildValidRecord();
        record.MarkAsStored();

        Assert.Throws<InvalidEhrRecordException>(() => record.MarkAsStored());
    }

    [Fact]
    public void MarkAsFailed_DebePassarADeadLetterAlSuperarMaxReintentos()
    {
        var record = BuildValidRecord();

        record.MarkAsFailed(maxRetries: 3);
        record.MarkAsFailed(maxRetries: 3);
        record.MarkAsFailed(maxRetries: 3);

        Assert.Equal(EhrRecordStatus.DeadLetter, record.Status);
        Assert.True(record.IsDeadLetter());
        Assert.Equal(3, record.RetryCount);
    }

    [Fact]
    public void MarkAsFailed_DebeEstarEnFailedAntesDelMaximo()
    {
        var record = BuildValidRecord();
        record.MarkAsFailed(maxRetries: 3);

        Assert.Equal(EhrRecordStatus.Failed, record.Status);
        Assert.False(record.IsDeadLetter());
    }

    // ── Builder ────────────────────────────────────────────────────────────

    [Fact]
    public void EhrRecordBuilder_DebeConstruirRegistroConTodosLosCampos()
    {
        var summary = new AppointmentSummary
        {
            InsuranceValidated = true,
            ProcedureCode      = "PROC-001"
        };

        var record = new EhrRecordBuilder()
            .WithAppointmentId(AppointmentId)
            .WithPatientId(PatientId)
            .WithDoctorId(DoctorId)
            .WithSpecialty(Specialty)
            .WithAppointmentSummary(summary)
            .WithCorrelationId(CorrelationId)
            .WithSourceService(SourceService)
            .Build();

        Assert.Equal(AppointmentId, record.AppointmentId);
        Assert.True(record.AppointmentSummary.InsuranceValidated);
    }

    // ── EhrRecordService ───────────────────────────────────────────────────

    [Fact]
    public void EhrRecordService_DebeConstruirRegistroPendiente()
    {
        var service = new EhrRecordService();
        var record  = service.BuildPendingRecord(
            AppointmentId, PatientId, DoctorId,
            Specialty, new AppointmentSummary(),
            CorrelationId, SourceService);

        Assert.Equal(EhrRecordStatus.Pending, record.Status);
        Assert.True(service.CanRetry(record));
    }

    // ── Helper ─────────────────────────────────────────────────────────────

    private static EhrRecord BuildValidRecord() =>
        EhrRecord.CreateFromAppointmentConfirmed(
            AppointmentId, PatientId, DoctorId, Specialty,
            new AppointmentSummary { InsuranceValidated = true },
            CorrelationId, SourceService);
}
