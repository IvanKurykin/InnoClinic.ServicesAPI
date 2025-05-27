using System.Data;
using Application.Extensions;
using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories;
using InnoClinic.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Infrastructure.Services;
using InnoClinic.Messaging.Enums;

namespace Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDbConnection>(_ =>
           new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

        services.AddMessagingForEntityType<Service>(MessageBrokerType.RabbitMQ);
        services.AddMessagingForEntityType<ServiceCategory>(MessageBrokerType.RabbitMQ);
        services.AddMessagingForEntityType<Specialization>(MessageBrokerType.RabbitMQ);

        services.AddDefaultMassTransit(configuration, MessageBrokerType.RabbitMQ);

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