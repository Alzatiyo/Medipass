using Aplication.Ports.In;
using Domain.Models;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Adapters.Rest;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentUseCasePort _useCase;

    public AppointmentController(
        IAppointmentUseCasePort useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
        CreateAppointmentRequest request)
    {
        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            Specialty = request.Specialty,
            AppointmentDate = request.AppointmentDate,
            ProcedureCode = request.ProcedureCode
        };

        var result =
            await _useCase.ScheduleAppointmentAsync(
                appointment);

        return Ok(result);
    }
}