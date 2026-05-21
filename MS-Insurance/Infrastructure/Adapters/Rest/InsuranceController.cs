using Aplication.Ports.In;
using Domain.Enums;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Adapters.Rest;

[ApiController]
[Route("api/[controller]")]
public class InsuranceController : ControllerBase
{
    private readonly IValidateInsuranceUseCase _useCase;

    public InsuranceController(IValidateInsuranceUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("validate")]
    public async Task<ActionResult<ValidateInsuranceResponse>> ValidateProcedure(
        [FromBody] ValidateInsuranceRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.InsuranceNumber) || string.IsNullOrWhiteSpace(request.ProcedureCode))
        {
            return BadRequest(new ValidateInsuranceResponse
            {
                Approved = false,
                Status = nameof(ValidationStatus.InvalidPolicy),
                Message = "El número de seguro y el código de procedimiento son obligatorios."
            });
        }

        var status = await _useCase.ValidateProcedureAsync(request.InsuranceNumber, request.ProcedureCode);

        var approved = status == ValidationStatus.Approved;
        var message = status switch
        {
            ValidationStatus.Approved => "El procedimiento está cubierto y pre-aprobado.",
            ValidationStatus.ProcedureNotCovered => $"El procedimiento '{request.ProcedureCode}' no está cubierto por el plan de seguros.",
            ValidationStatus.PlanInactive => "La póliza de seguros se encuentra inactiva o expirada.",
            ValidationStatus.InvalidPolicy => "La póliza de seguros no existe en el sistema.",
            _ => "Estado de validación desconocido."
        };

        return Ok(new ValidateInsuranceResponse
        {
            Approved = approved,
            Status = status.ToString(),
            Message = message
        });
    }

    [HttpGet("procedures")]
    public ActionResult<Dictionary<string, IEnumerable<string>>> GetProcedures()
    {
        var basicStrategy = Domain.Services.CoverageStrategyFactory.GetStrategy(PlanType.Basic);
        var premiumStrategy = Domain.Services.CoverageStrategyFactory.GetStrategy(PlanType.Premium);
        var oncologyStrategy = Domain.Services.CoverageStrategyFactory.GetStrategy(PlanType.Oncology);

        var result = new Dictionary<string, IEnumerable<string>>
        {
            { "Basic", basicStrategy.GetCoveredProcedures() },
            { "Premium", premiumStrategy.GetCoveredProcedures() },
            { "Oncology", oncologyStrategy.GetCoveredProcedures() }
        };

        return Ok(result);
    }
}
