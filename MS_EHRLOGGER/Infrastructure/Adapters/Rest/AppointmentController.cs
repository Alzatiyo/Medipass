using Microsoft.AspNetCore.Mvc;
using MsEhrLogger.Application.Ports.In;
using MsEhrLogger.Domain.Enums;
using MsEhrLogger.Domain.Exceptions;
using MsEhrLogger.Infrastructure.Dtos;
using MsEhrLogger.Infrastructure.Mappers.Interface;

namespace MsEhrLogger.Infrastructure.Adapters.Rest;

/// <summary>
/// Controlador REST de consulta para el Historial Clínico Digital.
/// Espejo de AppointmentController.cs del MS-AgendaHub.
///
/// Solo expone operaciones de LECTURA (CQRS read side).
/// La escritura ocurre exclusivamente vía el broker RabbitMQ.
/// </summary>
[ApiController]
[Route("api/v1/ehr-records")]
[Produces("application/json")]
public class AppointmentController : ControllerBase
{
    private readonly IEhrLoggerUseCasePort _useCase;
    private readonly IEhrRecordMapper      _mapper;

    public AppointmentController(
        IEhrLoggerUseCasePort useCase,
        IEhrRecordMapper      mapper)
    {
        _useCase = useCase;
        _mapper  = mapper;
    }

    /// <summary>Listar todos los registros EHR con paginación.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EhrRecordResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 0,
        [FromQuery] int size = 20)
    {
        var records = await _useCase.GetAllRecordsAsync(page, size);
        return Ok(records.Select(_mapper.ToResponse));
    }

    /// <summary>Obtener registro EHR por su ID único.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EhrRecordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var record = await _useCase.GetRecordByIdAsync(id);
        if (record is null)
            return NotFound(new { message = $"No se encontró registro EHR con id: {id}" });

        return Ok(_mapper.ToResponse(record));
    }

    /// <summary>Buscar el registro EHR asociado a una cita específica.</summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(EhrRecordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAppointmentId(string appointmentId)
    {
        var record = await _useCase.GetRecordByAppointmentIdAsync(appointmentId);
        if (record is null)
            return NotFound(new { message = $"No se encontró registro EHR para AppointmentId: {appointmentId}" });

        return Ok(_mapper.ToResponse(record));
    }

    /// <summary>Obtener el historial clínico completo de un paciente.</summary>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType(typeof(IEnumerable<EhrRecordResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPatient(string patientId)
    {
        var records = await _useCase.GetRecordsByPatientAsync(patientId);
        return Ok(records.Select(_mapper.ToResponse));
    }

    /// <summary>Filtrar registros EHR por estado de procesamiento.</summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<EhrRecordResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus(EhrRecordStatus status)
    {
        var records = await _useCase.GetRecordsByStatusAsync(status);
        return Ok(records.Select(_mapper.ToResponse));
    }
}
