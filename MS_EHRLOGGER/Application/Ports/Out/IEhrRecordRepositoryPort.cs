using MsEhrLogger.Domain.Enums;
using MsEhrLogger.Domain.Models;

namespace MsEhrLogger.Application.Ports.Out;

/// <summary>
/// Puerto de SALIDA para persistencia.
/// Espejo de IAppointmentRepositoryPort.cs del MS-AgendaHub.
///
/// DIP: el dominio depende de esta abstracción, nunca de MongoDB directamente.
/// </summary>
public interface IEhrRecordRepositoryPort
{
    Task<EhrRecord>              SaveAsync(EhrRecord record);
    Task<EhrRecord?>             FindByIdAsync(string id);
    Task<EhrRecord?>             FindByAppointmentIdAsync(string appointmentId);
    Task<IEnumerable<EhrRecord>> FindByPatientIdAsync(string patientId);
    Task<IEnumerable<EhrRecord>> FindByStatusAsync(EhrRecordStatus status);
    Task<IEnumerable<EhrRecord>> FindAllAsync(int page, int size);
    Task<long>                   CountByStatusAsync(EhrRecordStatus status);
}
