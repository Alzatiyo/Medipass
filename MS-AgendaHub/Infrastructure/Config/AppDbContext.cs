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

        // 2. Pacientes (Sincronización manual: mismos nombres/seguros que MS-Insurance)
        modelBuilder.Entity<PatientEntity>().HasData(
            new PatientEntity 
            { 
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), 
                FullName = "Diego Rodríguez", 
                InsuranceNumber = "INS-001" 
            },
            new PatientEntity 
            { 
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                FullName = "Juan Pérez", 
                InsuranceNumber = "INS-BASIC-ACTIVE" 
            },
            new PatientEntity 
            { 
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), 
                FullName = "Ana Gómez", 
                InsuranceNumber = "INS-PREMIUM-ACTIVE" 
            },
            new PatientEntity 
            { 
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                FullName = "Carlos Ruiz", 
                InsuranceNumber = "INS-ONCOLOGY-ACTIVE" 
            }
        );
    }
}