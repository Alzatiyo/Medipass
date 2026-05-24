using MsEhrLogger.Domain.Models;
using MsEhrLogger.Infrastructure.Adapters.Persistence;
using MsEhrLogger.Infrastructure.Dtos;

namespace MsEhrLogger.Infrastructure.Mappers.Interface;

/// <summary>
/// Interfaz del mapper EHR.
/// Espejo exacto de IAppointmentMapper.cs del MS-AgendaHub.
/// </summary>
public interface IEhrRecordMapper
{
    EhrRecord        ToDomain(EhrRecordEntity entity);
    EhrRecordEntity  ToEntity(EhrRecord domain);
    EhrRecordResponse ToResponse(EhrRecord domain);
}
