using Aplication.Ports.In;
using Aplication.Ports.Out;
using Aplication.UseCases;
using Domain.Services;
using Infrastructure.Adapters.Persistence;
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
        // Conexión SQL Server
        services.AddDbContext<InsuranceDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        // Mapeadores
        services.AddScoped<IInsurancePolicyMapper, InsurancePolicyMapper>();

        // Repositorios (Adapters de salida)
        services.AddScoped<IInsuranceRepositoryPort, InsuranceRepositoryAdapter>();

        // Servicios de Dominio
        services.AddScoped<InsuranceDomainService>();

        // Casos de Uso (Ports de entrada)
        services.AddScoped<IValidateInsuranceUseCase, ValidateInsuranceUseCase>();

        return services;
    }
}
