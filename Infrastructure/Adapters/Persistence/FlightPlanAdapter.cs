using Aplication.Ports.Out;
using Domain.Models;
using Infrastructure.Config;
using Infrastructure.Mappers.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Persistence;

public class FlightPlanAdapter : IFlightPlanRepositoryPort
{
    private readonly AppDbContext _context;
    private readonly IFlightPlanMapper _mapper;

    public FlightPlanAdapter(AppDbContext context, IFlightPlanMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FlightPlan> SaveAsync(FlightPlan plan)
    {
        var entity = _mapper.ToEntity(plan);
        _context.FlightPlans.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.ToDomain(entity);
    }

    public async Task<FlightPlan?> FindByIdAsync(Guid id)
    {
        var entity = await _context.FlightPlans.FindAsync(id);
        return entity is null ? null : _mapper.ToDomain(entity);
    }

    public async Task<IEnumerable<FlightPlan>> FindAllAsync()
    {
        var entities = await _context.FlightPlans.ToListAsync();
        return entities.Select(_mapper.ToDomain);
    }

    public async Task<FlightPlan> UpdateAsync(FlightPlan plan)
    {
        var entity = _mapper.ToEntity(plan);
        _context.FlightPlans.Update(entity);
        await _context.SaveChangesAsync();
        return _mapper.ToDomain(entity);
    }
}