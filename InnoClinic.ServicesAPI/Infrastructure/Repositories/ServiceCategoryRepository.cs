using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Helpers;

namespace Infrastructure.Repositories;

public class ServiceCategoryRepository(IDbConnection connection) : BaseRepository<ServiceCategory>(connection, "ServiceCategories"), IServiceCategoryRepository
{
    private readonly IDbConnection _connection = connection;

    public async Task<ServiceCategory?> GetWithServicesAsync(Guid id)
    {
        var dict = new Dictionary<Guid, ServiceCategory>();

        await _connection.QueryAsync<ServiceCategory, Service, ServiceCategory>(
            ServiceCategorySqlBuilder.GetByIdWithServices(),
            DapperMappingHelper.MapWithChildren<ServiceCategory, Service>(dict, c => c.Id, (c, list) => c.Services = list, (c, s) => ((List<Service>)c.Services!).Add(s)),
            new { Id = id },
            splitOn: DapperConstants.SplitOnId);

        return dict.Values.FirstOrDefault();
    }

    public async Task<IList<ServiceCategory>> GetAllWithServicesAsync()
    {
        var dict = new Dictionary<Guid, ServiceCategory>();

        await _connection.QueryAsync<ServiceCategory, Service, ServiceCategory>(
            ServiceCategorySqlBuilder.GetAllWithServices(),
            DapperMappingHelper.MapWithChildren<ServiceCategory, Service>(dict, c => c.Id, (c, list) => c.Services = list, (c, s) => ((List<Service>)c.Services!).Add(s)),
            splitOn: DapperConstants.SplitOnId);

        return dict.Values.ToList();
    }

    public async Task DeleteWithServicesAsync(Guid id) =>
        await _connection.ExecuteAsync(ServiceCategorySqlBuilder.DeleteWithServices(), new { Id = id });
}