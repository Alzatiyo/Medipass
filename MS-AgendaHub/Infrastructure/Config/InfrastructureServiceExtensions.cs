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
/// Registra: PostgreSQL, RabbitMQ, Adaptadores, Mappers y Casos de uso.
/// </summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        // Configuracion PostgreSQL

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(
                    "DefaultConnection")));

        // Mappers

        services.AddScoped<IAppointmentMapper,
            AppointmentMapper>();

        // Repositorios

        services.AddScoped<IAppointmentRepositoryPort,
            AppointmentRepositoryAdapter>();

        // Servicios externos

        services.AddHttpClient<IInsuranceServicePort, InsuranceServiceAdapter>();

        services.AddScoped<IDoctorAvailabilityPort,
            DoctorAvailabilityAdapter>();

        services.AddScoped<IEhrEventPublisherPort,
            EhrEventPublisherAdapter>();

        // Casos de uso

        services.AddScoped<IAppointmentUseCasePort,
            AppointmentUseCase>();

        return services;
    }
        services
            .AddDatabase(configuration)
            .AddRabbitMQ(configuration)
            .AddAdapters()
            .AddMappers()
            .AddUseCases();

        return services;
    }

    // ── PostgreSQL ─────────────────────────────────────────────────────────

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgreSQL")
                ?? "Host=localhost;Port=5432;Database=agendahub_db;Username=agendahub_user;Password=agendahub_pass"));

        services.AddScoped<IAppointmentRepositoryPort, AppointmentRepositoryAdapter>();
        return services;
    }

    // ── RabbitMQ ───────────────────────────────────────────────────────────
    // Singleton: una sola conexión TCP compartida por toda la aplicación.
    // IModel (channel) es creado dentro de cada adapter que lo necesite.

    private static IServiceCollection AddRabbitMQ(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
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

            return factory.CreateConnection("ms-agendahub");  // Nombre visible en RabbitMQ UI
        });

        // Singleton: el channel y la conexión se reutilizan durante toda la vida de la app
        services.AddSingleton<IEhrEventPublisherPort, EhrEventPublisherAdapter>();

        return services;
    }

    // ── Adaptadores REST externos ──────────────────────────────────────────

    private static IServiceCollection AddAdapters(
        this IServiceCollection services)
    {
        services.AddHttpClient<IInsuranceServicePort, InsuranceServiceAdapter>();
        services.AddHttpClient<IDoctorAvailabilityPort, DoctorAvailabilityAdapter>();
        return services;
    }

    // ── Mappers ────────────────────────────────────────────────────────────

    private static IServiceCollection AddMappers(
        this IServiceCollection services)
    {
        services.AddSingleton<IAppointmentMapper, AppointmentMapper>();
        return services;
    }

    // ── Casos de uso (Application Layer) ──────────────────────────────────

    private static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddScoped<IAppointmentUseCasePort, AppointmentUseCase>();
        return services;
    }
}
