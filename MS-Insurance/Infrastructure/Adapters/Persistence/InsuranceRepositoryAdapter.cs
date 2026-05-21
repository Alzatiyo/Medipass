using Aplication.Ports.Out;
using Domain.Models;
using Infrastructure.Mappers.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Persistence;

public class InsuranceRepositoryAdapter : IInsuranceRepositoryPort
{
    private readonly InsuranceDbContext _context;
    private readonly IInsurancePolicyMapper _mapper;

    public InsuranceRepositoryAdapter(
        InsuranceDbContext context,
        IInsurancePolicyMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InsurancePolicy?> GetByInsuranceNumberAsync(string insuranceNumber)
    {
        var entity = await _context.InsurancePolicies
            .FirstOrDefaultAsync(p => p.InsuranceNumber == insuranceNumber);

        return _mapper.ToDomain(entity);
    }

    public async Task SaveAsync(InsurancePolicy policy)
    {
        var entity = _mapper.ToEntity(policy);
        if (entity == null) return;

        var existingEntity = await _context.InsurancePolicies.FindAsync(entity.Id);
        if (existingEntity == null)
        {
            _context.InsurancePolicies.Add(entity);
        }
        else
        {
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        }

        await _context.SaveChangesAsync();
    }
}
