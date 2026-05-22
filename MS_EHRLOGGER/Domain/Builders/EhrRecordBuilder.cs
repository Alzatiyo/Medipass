using MsEhrLogger.Domain.Models;

namespace MsEhrLogger.Domain.Builders;

/// <summary>
/// Patrón GoF: BUILDER
/// Construye un EhrRecord paso a paso desde los datos del evento recibido.
/// Espejo exacto de AppointmentBuilder.cs del MS-AgendaHub.
/// </summary>
public class EhrRecordBuilder
{
    private string            _appointmentId   = string.Empty;
    private string            _patientId       = string.Empty;
    private string            _doctorId        = string.Empty;
    private string            _specialty       = string.Empty;
    private string            _correlationId   = string.Empty;
    private string            _sourceService   = string.Empty;
    private AppointmentSummary _summary        = new();

    public EhrRecordBuilder WithAppointmentId(string appointmentId)
    {
        _appointmentId = appointmentId;
        return this;
    }

    public EhrRecordBuilder WithPatientId(string patientId)
    {
        _patientId = patientId;
        return this;
    }

    public EhrRecordBuilder WithDoctorId(string doctorId)
    {
        _doctorId = doctorId;
        return this;
    }

    public EhrRecordBuilder WithSpecialty(string specialty)
    {
        _specialty = specialty;
        return this;
    }

    public EhrRecordBuilder WithCorrelationId(string correlationId)
    {
        _correlationId = correlationId;
        return this;
    }

    public EhrRecordBuilder WithSourceService(string sourceService)
    {
        _sourceService = sourceService;
        return this;
    }

    public EhrRecordBuilder WithAppointmentSummary(AppointmentSummary summary)
    {
        _summary = summary;
        return this;
    }

    public EhrRecord Build() =>
        EhrRecord.CreateFromAppointmentConfirmed(
            _appointmentId,
            _patientId,
            _doctorId,
            _specialty,
            _summary,
            _correlationId,
            _sourceService);
}
