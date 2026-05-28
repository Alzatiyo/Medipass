using Aplication.Ports.Out;
using Domain.Models;
using Infrastructure.Config;
using Infrastructure.Mappers.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Persistence;

public class AppointmentRepositoryAdapter
    : IAppointmentRepositoryPort
{
    private readonly AppDbContext _context;

    private readonly IAppointmentMapper _mapper;

    public AppointmentRepositoryAdapter(
        AppDbContext context,
        IAppointmentMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task SaveAsync(Appointment appointment)
    {
        var entity = _mapper.ToEntity(appointment);

        await _context.Appointments.AddAsync(entity);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAppointmentAsync(
        Guid doctorId,
        DateTime appointmentDate)
    {
        return await _context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.AppointmentDate == appointmentDate);
    }

    public async Task<string?> GetPatientInsuranceNumberAsync(Guid patientId)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        return patient?.InsuranceNumber;
    }
}