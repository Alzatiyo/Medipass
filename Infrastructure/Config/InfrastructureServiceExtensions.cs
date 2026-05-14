using Aplication.Ports.In;
using Aplication.Ports.Out;
using Aplication.UseCases;
using Domain.Services;
using Infrastructure.Adapters.Persistence;
using Infrastructure.Adapters.Rest;
using Infrastructure.Mappers;
using Infrastructure.Mappers.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Config;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core — SQL Server (swap to TimescaleDB/PostgreSQL in prod)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // HTTP clients for sync microservices
        //services.AddHttpClient<IWeatherMonitorPort, WeatherMonitorAdapter>(client =>
        //{
        //    client.BaseAddress = new Uri(
        //        configuration["Services:WeatherMonitor"]
        //        ?? "http://localhost:5001");
        //});

        //services.AddHttpClient<INoFlyZonePort, NoFlyZoneAdapter>(client =>
        //{
        //    client.BaseAddress = new Uri(
        //        configuration["Services:NoFlyZone"]
        //        ?? "http://localhost:5002");
        //});
        services.AddScoped<IWeatherMonitorPort, WeatherMonitorStub>();
        services.AddScoped<INoFlyZonePort, NoFlyZoneStub>();
        services.AddScoped<IDeliveryProofPort, DeliveryProofStub>();

        // Domain & Application
        services.AddScoped<FlightPlanService>();
        services.AddScoped<IFlightPlanRepositoryPort, FlightPlanAdapter>();
        //services.AddScoped<IDeliveryProofPort, DeliveryProofAdapter>();
        services.AddScoped<IFlightPlanUseCasePort, FlightPlanUseCase>();

        // Mappers
        services.AddScoped<IFlightPlanMapper, FlightPlanMapper>();

        return services;
    }
}