namespace MsEhrLogger.Domain.Exceptions;

/// <summary>
/// Excepción base del dominio EHR.
/// Misma estructura que DomainException.cs del MS-AgendaHub.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>
/// Registro EHR no encontrado.
/// </summary>
public class EhrRecordNotFoundException : DomainException
{
    public EhrRecordNotFoundException(string id)
        : base($"No se encontró registro EHR con id: {id}") { }

    public EhrRecordNotFoundException(string field, string value)
        : base($"No se encontró registro EHR con {field}: {value}") { }
}

/// <summary>
/// Datos del registro EHR inválidos (viola invariantes de dominio).
/// </summary>
public class InvalidEhrRecordException : DomainException
{
    public InvalidEhrRecordException(string message) : base(message) { }
}
