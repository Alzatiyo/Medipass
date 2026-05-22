using MongoDB.Driver;
using MsEhrLogger.Application.Ports.In;
using MsEhrLogger.Application.Ports.Out;
using MsEhrLogger.Application.UseCases;
using MsEhrLogger.Domain.Services;
using MsEhrLogger.Infrastructure.Adapters.Persistence;
using MsEhrLogger.Infrastructure.Adapters.Rest;
using MsEhrLogger.Infrastructure.Mappers;
using MsEhrLogger.Infrastructure.Mappers.Interface;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;

namespace MsEhrLogger.Infrastructure.Config;

/// <summary>
/// Registro de dependencias de infraestructura.
/// Espejo exacto de InfrastructureServiceExtensions.cs del MS-AgendaHub.
/// Centraliza toda la inyección de dependencias en un método de extensión.
/// </summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        services
            .AddMongoDB(configuration)
            .AddRabbitMQ(configuration)
            .AddApplicationServices()
            .AddObservability(configuration);

        return services;
    }

    // ── MongoDB ────────────────────────────────────────────────────────────

    private static IServiceCollection AddMongoDB(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        services.AddSingleton<AppDbContext>();
        services.AddSingleton<IMongoDatabase>(sp =>
            sp.GetRequiredService<AppDbContext>().Database);

        services.AddSingleton<IEhrRecordRepositoryPort, EhrRecordRepositoryAdapter>();
        return services;
    }

    // ── RabbitMQ ───────────────────────────────────────────────────────────

    private static IServiceCollection AddRabbitMQ(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        services.AddSingleton<IConnection>(_ =>
        {
            var factory = new ConnectionFactory
            {
                HostName         = configuration["RabbitMQ:Host"]     ?? "localhost",
                Port             = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                UserName         = configuration["RabbitMQ:Username"]  ?? "medipass",
                Password         = configuration["RabbitMQ:Password"]  ?? "medipass_pass",
                VirtualHost      = "/",
                DispatchConsumersAsync = true // Necesario para AsyncEventingBasicConsumer
            };
            return factory.CreateConnection();
        });

        services.AddSingleton<IEhrEventPublisherPort, EhrEventPublisherAdapter>();
        services.AddHostedService<AppointmentConsumerAdapter>();
        return services;
    }

    // ── Servicios de aplicación y dominio ─────────────────────────────────

    private static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        // Mapper — espejo de AppointmentMapper registration
        services.AddSingleton<IEhrRecordMapper, EhrRecordMapper>();

        // Servicio de dominio puro
        services.AddSingleton<EhrRecordService>();

        // Caso de uso — espejo de AppointmentUseCase registration
        services.AddScoped<IEhrLoggerUseCasePort, EhrLoggerUseCase>();

        return services;
    }

    // ── Observabilidad: OpenTelemetry / Jaeger ────────────────────────────

    private static IServiceCollection AddObservability(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        var jaegerEndpoint = configuration["Jaeger:Endpoint"] ?? "http://localhost:14268/api/traces";

        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("ms-ehrlogger"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter(opts =>
                    {
                        opts.AgentHost = configuration["Jaeger:Host"] ?? "localhost";
                        opts.AgentPort = int.Parse(configuration["Jaeger:Port"] ?? "6831");
                    });
            });

        return services;
    }
}
