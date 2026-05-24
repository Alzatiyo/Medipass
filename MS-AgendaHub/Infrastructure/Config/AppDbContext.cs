using Infrastructure.Adapters.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppointmentEntity> Appointments =>
        Set<AppointmentEntity>();

    public DbSet<DoctorEntity> Doctors =>
        Set<DoctorEntity>();

    public DbSet<PatientEntity> Patients =>
        Set<PatientEntity>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        // Evitar doble agendamiento

        modelBuilder.Entity<AppointmentEntity>()
            .HasIndex(a => new
            {
                a.DoctorId,
                a.AppointmentDate
            })
            .IsUnique();

        // ── Datos Semilla (Seed Data) ──────────────────────────────────────────

        // 1. Médicos
        modelBuilder.Entity<DoctorEntity>().HasData(
            new DoctorEntity 
            { 
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7"), 
                FullName = "Dra. Sara Connor", 
                Specialty = "Medicina General" 
            },
            new DoctorEntity 
            { 
                Id = Guid.Parse("5ba85f64-5717-4562-b3fc-2c963f66afa8"), 
                FullName = "Dr. Gregory House", 
                Specialty = "Cardiología" 
            }
        );

        // 2. Pacientes (Sincronización exacta con MS-Insurance)
        modelBuilder.Entity<PatientEntity>().HasData(
            new PatientEntity 
            { 
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), 
                FullName = "Diego Rodríguez", 
                InsuranceNumber = "INS-001" 
            },
            new PatientEntity 
            { 
                Id = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), 
                FullName = "Juan Pérez", 
                InsuranceNumber = "INS-BASIC-ACTIVE" 
            },
            new PatientEntity 
            { 
                Id = Guid.Parse("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"), 
                FullName = "Ana Gómez", 
                InsuranceNumber = "INS-PREMIUM-ACTIVE" 
            },
            new PatientEntity 
            { 
                Id = Guid.Parse("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), 
                FullName = "Carlos Ruiz", 
                InsuranceNumber = "INS-ONCOLOGY-ACTIVE" 
            },
            new PatientEntity 
            { 
                Id = Guid.Parse("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"), 
                FullName = "María López", 
                InsuranceNumber = "INS-INACTIVE" 
            },
            new PatientEntity 
            { 
                Id = Guid.Parse("5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b"), 
                FullName = "Luis Silva", 
                InsuranceNumber = "INS-EXPIRED" 
            }
        );
    }
}