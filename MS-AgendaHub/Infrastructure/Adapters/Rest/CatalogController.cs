using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Rest;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly AppDbContext _context;

    public CatalogController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("doctors")]
    public async Task<IActionResult> GetDoctors()
    {
        var doctors = await _context.Doctors.ToListAsync();
        return Ok(doctors);
    }

    [HttpGet("patients")]
    public async Task<IActionResult> GetPatients()
    {
        var patients = await _context.Patients.ToListAsync();
        return Ok(patients);
    }
}
