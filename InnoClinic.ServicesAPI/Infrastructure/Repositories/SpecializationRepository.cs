using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Helpers;
using System.Data;

namespace Infrastructure.Repositories;

public class SpecializationRepository(IDbConnection connection) : BaseRepository<Specialization>(connection, "Specializations"), ISpecializationRepository
{
    private readonly IDbConnection _connection = connection;

    public async Task<Specialization?> GetWithServicesAsync(Guid id)
    {
        var dict = new Dictionary<Guid, Specialization>();

        await _connection.QueryAsync<Specialization, Service, Specialization>(
            SpecializationSqlBuilder.GetByIdWithServices(),
            DapperMappingHelper.MapWithChildren<Specialization, Service>(dict, s => s.Id, (s, list) => s.Services = list, (s, svc) => ((List<Service>)s.Services!).Add(svc)),
            new { Id = id },
            splitOn: DapperConstants.SplitOnId);

        return dict.Values.FirstOrDefault();
    }

    public async Task<IList<Specialization>> GetAllWithServicesAsync()
    {
        var dict = new Dictionary<Guid, Specialization>();

        await _connection.QueryAsync<Specialization, Service, Specialization>(
            SpecializationSqlBuilder.GetAllWithServices(),
            DapperMappingHelper.MapWithChildren<Specialization, Service>(dict, s => s.Id, (s, list) => s.Services = list, (s, svc) => ((List<Service>)s.Services!).Add(svc)
            ),
            splitOn: DapperConstants.SplitOnId);

        return dict.Values.ToList();
    }

    public async Task DeleteWithServicesAsync(Guid id) =>
        await _connection.ExecuteAsync(SpecializationSqlBuilder.DeleteWithServices(), new { Id = id });
}