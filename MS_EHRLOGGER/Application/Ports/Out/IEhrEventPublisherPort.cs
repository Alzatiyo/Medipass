using MsEhrLogger.Domain.Models;

namespace MsEhrLogger.Application.Ports.Out;

/// <summary>
/// Puerto de SALIDA para publicación de eventos de auditoría.
/// Mismo nombre que en MS-AgendaHub (IEhrEventPublisherPort.cs),
/// pero en este MS es el EHRLogger quien publica confirmaciones de vuelta.
///
/// Patrón Observer: notifica a suscriptores sin acoplar el caso de uso.
/// </summary>
public interface IEhrEventPublisherPort
{
    Task PublishRecordStoredAsync(EhrRecord record);
    Task PublishRecordFailedAsync(EhrRecord record, string errorReason);
}
