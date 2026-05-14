using Infrastructure.Adapters.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<FlightPlanEntity> FlightPlans => Set<FlightPlanEntity>();
    public DbSet<NoFlyZoneEntity> NoFlyZones => Set<NoFlyZoneEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlightPlanEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.DroneId).IsRequired().HasMaxLength(100);
            e.Property(x => x.OriginZone).IsRequired().HasMaxLength(200);
            e.Property(x => x.DestinationZone).IsRequired().HasMaxLength(200);
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.ZoneType).HasConversion<string>();
        });

        modelBuilder.Entity<NoFlyZoneEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ZoneName).IsRequired().HasMaxLength(200);
            e.Property(x => x.ZoneType).HasConversion<string>();
        });
    }
}