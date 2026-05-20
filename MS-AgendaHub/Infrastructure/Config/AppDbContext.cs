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
    }
}