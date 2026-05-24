using Aplication.Ports.In;
using Aplication.Ports.Out;
using Aplication.UseCases;
using Infrastructure.Adapters.Persistence;
using Infrastructure.Adapters.Rest;
using Infrastructure.Mappers;
using Infrastructure.Mappers.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure.Config;

/// <summary>
/// Extensión de registro de dependencias de infraestructura.
/// Un único punto de configuración para toda la capa de infraestructura.
///
/// Registra: SQL Server, RabbitMQ, Adaptadores, Mappers y Casos de uso.
/// </summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        // ── Base de Datos (SQL Server) ─────────────────────────────────────────

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        // ── Repositorios ───────────────────────────────────────────────────────

        services.AddScoped<IAppointmentRepositoryPort, AppointmentRepositoryAdapter>();

        // ── RabbitMQ ───────────────────────────────────────────────────────────

        services.AddSingleton<IConnection>(_ =>
        {
            var factory = new ConnectionFactory
            {
                HostName    = configuration["RabbitMQ:Host"]     ?? "localhost",
                Port        = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                UserName    = configuration["RabbitMQ:Username"] ?? "medipass",
                Password    = configuration["RabbitMQ:Password"] ?? "medipass_pass",
                VirtualHost = "/"
            };

            return factory.CreateConnection("ms-agendahub");
        });

        // ── Adaptadores REST y de Mensajería ───────────────────────────────────

        services.AddHttpClient<IInsuranceServicePort, InsuranceServiceAdapter>();
        services.AddScoped<IDoctorAvailabilityPort, DoctorAvailabilityAdapter>();
        
        // IEhrEventPublisherPort debe ser Singleton porque inyecta IConnection (que es Singleton)
        services.AddSingleton<IEhrEventPublisherPort, EhrEventPublisherAdapter>();

        // ── Mappers ────────────────────────────────────────────────────────────

        services.AddScoped<IAppointmentMapper, AppointmentMapper>();

        // ── Casos de uso ───────────────────────────────────────────────────────

        services.AddScoped<IAppointmentUseCasePort, AppointmentUseCase>();

        return services;
    }
}
