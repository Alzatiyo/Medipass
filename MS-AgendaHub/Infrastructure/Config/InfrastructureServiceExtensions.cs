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

namespace Infrastructure.Config;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuración PostgreSQL

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

        services.AddScoped<IInsuranceServicePort,
            InsuranceServiceAdapter>();

        services.AddScoped<IDoctorAvailabilityPort,
            DoctorAvailabilityAdapter>();

        services.AddScoped<IEhrEventPublisherPort,
            EhrEventPublisherAdapter>();

        // Casos de uso

        services.AddScoped<IAppointmentUseCasePort,
            AppointmentUseCase>();

        return services;
    }
}