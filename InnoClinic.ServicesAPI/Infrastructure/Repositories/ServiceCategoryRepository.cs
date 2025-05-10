using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Helpers;
using Infrastructure.Helpers.Constants;

namespace Infrastructure.Repositories;

public class ServiceCategoryRepository(IDbConnection connection) : BaseRepository<ServiceCategory>(connection, "ServiceCategories"), IServiceCategoryRepository
{
    private readonly IDbConnection _connection = connection;

    public async Task<ServiceCategory?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dict = new Dictionary<Guid, ServiceCategory>();

        await _connection.QueryAsync<ServiceCategory, Service, ServiceCategory>(
            ServiceCategorySqlBuilder.GetByIdWithDependencies(),
            DapperMappingHelper.MapWithChildren<ServiceCategory, Service>(dict, c => c.Id, (c, list) => c.Services = list, (c, s) => ((List<Service>)c.Services!).Add(s)),
            new { Id = id },
            splitOn: Helpers.Constants.DapperConstants.SplitOnId);

        return dict.Values.FirstOrDefault();
    }

    public async Task<IList<ServiceCategory>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default)
    {
        var dict = new Dictionary<Guid, ServiceCategory>();

        await _connection.QueryAsync<ServiceCategory, Service, ServiceCategory>(
            ServiceCategorySqlBuilder.GetAllWithDependencies(),
            DapperMappingHelper.MapWithChildren<ServiceCategory, Service>(dict, c => c.Id, (c, list) => c.Services = list, (c, s) => ((List<Service>)c.Services!).Add(s)),
            splitOn: Helpers.Constants.DapperConstants.SplitOnId);

        return dict.Values.ToList();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _connection.ExecuteAsync(ServiceCategorySqlBuilder.Delete(), new { Id = id });
}