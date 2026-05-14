using Aplication.Ports.In;
using Domain.Builders;
using Domain.Enums;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Adapters.Rest;

[ApiController]
[Route("api/[controller]")]
public class FlightPlanController : ControllerBase
{
    private readonly IFlightPlanUseCasePort _useCase;

    public FlightPlanController(IFlightPlanUseCasePort useCase)
    {
        _useCase = useCase;
    }

    /// <summary>GET all flight plans</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var plans = await _useCase.GetAllFlightPlansAsync();
        return Ok(plans);
    }

    /// <summary>GET a single flight plan by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var plan = await _useCase.GetFlightPlanAsync(id);
        return Ok(plan);
    }

    /// <summary>
    /// POST — Submit a new flight plan.
    /// Synchronously validates weather and no-fly zones before authorizing.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] CreateFlightPlanRequest request)
    {
        var plan = new FlightPlanBuilder()
            .WithDroneId(request.DroneId)
            .WithOriginZone(request.OriginZone)
            .WithDestinationZone(request.DestinationZone)
            .WithAltitude(request.AltitudeMeters)
            .WithZoneType(request.ZoneType)
            .WithStatus(FlightPlanStatus.Pending)
            .Build();

        var result = await _useCase.SubmitFlightPlanAsync(plan);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// PATCH — Register delivery with proof image.
    /// Asynchronously publishes the proof to MS-DeliveryProof.
    /// </summary>
    [HttpPatch("{id:guid}/delivery")]
    public async Task<IActionResult> RegisterDelivery(
        Guid id, [FromBody] RegisterDeliveryRequest request)
    {
        var result = await _useCase.RegisterDeliveryAsync(id, request.ProofImageUrl);
        return Ok(result);
    }

    /// <summary>PATCH — Update flight plan status (e.g. InFlight, Cancelled)</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid id, [FromBody] UpdateFlightStatusRequest request)
    {
        var result = await _useCase.UpdateFlightStatusAsync(id, request.Status);
        return Ok(result);
    }
}