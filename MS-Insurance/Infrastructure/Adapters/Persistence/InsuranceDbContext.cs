using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Persistence;

public class InsuranceDbContext : DbContext
{
    public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
        : base(options)
    {
    }

    public DbSet<InsurancePolicyEntity> InsurancePolicies => Set<InsurancePolicyEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InsurancePolicyEntity>()
            .HasIndex(p => p.InsuranceNumber)
            .IsUnique();

        // Datos semilla
        modelBuilder.Entity<InsurancePolicyEntity>().HasData(
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                InsuranceNumber = "INS-001",
                PatientName = "Diego Rodríguez",
                Plan = "Basic",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(10)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                InsuranceNumber = "INS-BASIC-ACTIVE",
                PatientName = "Juan Pérez",
                Plan = "Basic",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(5)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                InsuranceNumber = "INS-PREMIUM-ACTIVE",
                PatientName = "Ana Gómez",
                Plan = "Premium",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(5)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                InsuranceNumber = "INS-ONCOLOGY-ACTIVE",
                PatientName = "Carlos Ruiz",
                Plan = "Oncology",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(5)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                InsuranceNumber = "INS-INACTIVE",
                PatientName = "María López",
                Plan = "Premium",
                IsActive = false,
                ExpirationDate = DateTime.UtcNow.AddYears(2)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                InsuranceNumber = "INS-EXPIRED",
                PatientName = "Luis Silva",
                Plan = "Basic",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(-1)
            }
        );
    }
}
