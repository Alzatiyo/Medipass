using MsEhrLogger.Domain.Enums;
using MsEhrLogger.Domain.Models;
using MsEhrLogger.Infrastructure.Dtos;

namespace MsEhrLogger.Application.Ports.In;

/// <summary>
/// Puerto de ENTRADA (Inbound Port).
/// Espejo de IAppointmentUseCasePort.cs del MS-AgendaHub.
///
/// Define las operaciones que el dominio expone al exterior.
/// </summary>
public interface IEhrLoggerUseCasePort
{
    /// <summary>
    /// Regla de Negocio #3: procesa el evento de cita confirmada
    /// publicado por MS-AgendaHub de forma asíncrona.
    /// </summary>
    Task<EhrRecord> ProcessAppointmentConfirmedAsync(AppointmentConfirmedEvent ehrEvent);

    Task<EhrRecord?>             GetRecordByIdAsync(string id);
    Task<EhrRecord?>             GetRecordByAppointmentIdAsync(string appointmentId);
    Task<IEnumerable<EhrRecord>> GetRecordsByPatientAsync(string patientId);
    Task<IEnumerable<EhrRecord>> GetRecordsByStatusAsync(EhrRecordStatus status);
    Task<IEnumerable<EhrRecord>> GetAllRecordsAsync(int page, int size);
}
