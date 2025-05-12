using System.Diagnostics.CodeAnalysis;
using Application.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationExtension
{
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceMapperProfile));
        services.AddAutoMapper(typeof(ServiceCategoryMapperProfile));
        services.AddAutoMapper(typeof(SpecializationMapperProfile));

        return services;
    }
}