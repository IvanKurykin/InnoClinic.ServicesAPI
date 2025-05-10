using System.Data;
using Application.Extensions;
using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDbConnection>(_ =>
           new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();

        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
        services.AddScoped<ISpecializationService, SpecializationService>();

        services.AddApplicationLayerServices();

        return services;
    }
}