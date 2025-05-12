using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Helpers;
using Infrastructure.Helpers.Builders;
using Infrastructure.Helpers.Constants;
using System.Data;

namespace Infrastructure.Repositories;

public class SpecializationRepository(IDbConnection connection) : BaseRepository<Specialization>(connection, "Specializations"), ISpecializationRepository
{
    private readonly IDbConnection _connection = connection;

    public async Task<Specialization?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dict = new Dictionary<Guid, Specialization>();

        await _connection.QueryAsync<Specialization, Service, Specialization>(
            SpecializationSqlBuilder.GetByIdWithDependencies(),
            DapperMappingHelper.MapWithChildren<Specialization, Service>(dict, s => s.Id, (s, list) => s.Services = list, (s, svc) => ((List<Service>)s.Services!).Add(svc)),
            new { Id = id },
            splitOn: DapperConstants.SplitOnId);

        return dict.Values.FirstOrDefault();
    }

    public async Task<IReadOnlyCollection<Specialization>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default)
    {
        var dict = new Dictionary<Guid, Specialization>();

        await _connection.QueryAsync<Specialization, Service, Specialization>(
            SpecializationSqlBuilder.GetAllWithDependencies(),
            DapperMappingHelper.MapWithChildren<Specialization, Service>(dict, s => s.Id, (s, list) => s.Services = list, (s, svc) => ((List<Service>)s.Services!).Add(svc)
            ),
            splitOn: DapperConstants.SplitOnId);

        return dict.Values.ToList();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _connection.ExecuteAsync(SpecializationSqlBuilder.Delete(), new { Id = id });
}