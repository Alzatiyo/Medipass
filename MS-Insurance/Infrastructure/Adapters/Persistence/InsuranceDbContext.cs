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
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                InsuranceNumber = "INS-001",
                PatientName = "Diego Rodríguez",
                Plan = "Basic",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(10)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
                InsuranceNumber = "INS-BASIC-ACTIVE",
                PatientName = "Juan Pérez",
                Plan = "Basic",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(5)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"),
                InsuranceNumber = "INS-PREMIUM-ACTIVE",
                PatientName = "Ana Gómez",
                Plan = "Premium",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(5)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"),
                InsuranceNumber = "INS-ONCOLOGY-ACTIVE",
                PatientName = "Carlos Ruiz",
                Plan = "Oncology",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(5)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"),
                InsuranceNumber = "INS-INACTIVE",
                PatientName = "María López",
                Plan = "Premium",
                IsActive = false,
                ExpirationDate = DateTime.UtcNow.AddYears(2)
            },
            new InsurancePolicyEntity
            {
                Id = Guid.Parse("5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b"),
                InsuranceNumber = "INS-EXPIRED",
                PatientName = "Luis Silva",
                Plan = "Basic",
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddYears(-1)
            }
        );
    }
}
