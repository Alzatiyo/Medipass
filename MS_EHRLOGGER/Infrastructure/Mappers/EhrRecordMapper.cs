using MsEhrLogger.Domain.Models;
using MsEhrLogger.Infrastructure.Adapters.Persistence;
using MsEhrLogger.Infrastructure.Dtos;
using MsEhrLogger.Infrastructure.Mappers.Interface;

namespace MsEhrLogger.Infrastructure.Mappers;

/// <summary>
/// Mapper concreto: dominio ↔ entidad MongoDB ↔ DTO HTTP.
/// Espejo exacto de AppointmentMapper.cs del MS-AgendaHub.
/// Anti-Corruption Layer entre las tres representaciones.
/// </summary>
public class EhrRecordMapper : IEhrRecordMapper
{
    // ── Entidad → Dominio ──────────────────────────────────────────────────

    public EhrRecord ToDomain(EhrRecordEntity entity)
    {
        var summary = entity.AppointmentSummary is null ? new AppointmentSummary() :
            new AppointmentSummary
            {
                ScheduledAt        = entity.AppointmentSummary.ScheduledAt,
                ConsultationRoom   = entity.AppointmentSummary.ConsultationRoom,
                InsuranceCode      = entity.AppointmentSummary.InsuranceCode,
                ProcedureCode      = entity.AppointmentSummary.ProcedureCode,
                Observations       = entity.AppointmentSummary.Observations,
                InsuranceValidated = entity.AppointmentSummary.InsuranceValidated
            };

        // Reconstruye el aggregate root respetando su estado persistido
        return EhrRecord.Reconstitute(
            entity.Id,
            entity.AppointmentId,
            entity.PatientId,
            entity.DoctorId,
            entity.Specialty,
            entity.EventType,
            entity.Status,
            summary,
            entity.OccurredAt,
            entity.ProcessedAt,
            entity.SourceService,
            entity.CorrelationId,
            entity.RetryCount);
    }

    // ── Dominio → Entidad ──────────────────────────────────────────────────

    public EhrRecordEntity ToEntity(EhrRecord domain)
    {
        AppointmentSummaryDocument? summaryDoc = null;
        if (domain.AppointmentSummary is not null)
        {
            summaryDoc = new AppointmentSummaryDocument
            {
                ScheduledAt        = domain.AppointmentSummary.ScheduledAt,
                ConsultationRoom   = domain.AppointmentSummary.ConsultationRoom,
                InsuranceCode      = domain.AppointmentSummary.InsuranceCode,
                ProcedureCode      = domain.AppointmentSummary.ProcedureCode,
                Observations       = domain.AppointmentSummary.Observations,
                InsuranceValidated = domain.AppointmentSummary.InsuranceValidated
            };
        }

        return new EhrRecordEntity
        {
            Id                 = domain.Id,
            AppointmentId      = domain.AppointmentId,
            PatientId          = domain.PatientId,
            DoctorId           = domain.DoctorId,
            Specialty          = domain.Specialty,
            EventType          = domain.EventType,
            Status             = domain.Status,
            AppointmentSummary = summaryDoc,
            OccurredAt         = domain.OccurredAt,
            ProcessedAt        = domain.ProcessedAt,
            SourceService      = domain.SourceService,
            CorrelationId      = domain.CorrelationId,
            RetryCount         = domain.RetryCount
        };
    }

    // ── Dominio → DTO HTTP ─────────────────────────────────────────────────

    public EhrRecordResponse ToResponse(EhrRecord domain)
    {
        AppointmentSummaryDto? summaryDto = null;
        if (domain.AppointmentSummary is not null)
        {
            summaryDto = new AppointmentSummaryDto
            {
                ScheduledAt        = domain.AppointmentSummary.ScheduledAt,
                ConsultationRoom   = domain.AppointmentSummary.ConsultationRoom,
                InsuranceCode      = domain.AppointmentSummary.InsuranceCode,
                ProcedureCode      = domain.AppointmentSummary.ProcedureCode,
                Observations       = domain.AppointmentSummary.Observations,
                InsuranceValidated = domain.AppointmentSummary.InsuranceValidated
            };
        }

        return new EhrRecordResponse
        {
            Id                 = domain.Id,
            AppointmentId      = domain.AppointmentId,
            PatientId          = domain.PatientId,
            DoctorId           = domain.DoctorId,
            Specialty          = domain.Specialty,
            EventType          = domain.EventType,
            Status             = domain.Status,
            AppointmentSummary = summaryDto,
            OccurredAt         = domain.OccurredAt,
            ProcessedAt        = domain.ProcessedAt,
            SourceService      = domain.SourceService,
            CorrelationId      = domain.CorrelationId,
            RetryCount         = domain.RetryCount
        };
    }
}
