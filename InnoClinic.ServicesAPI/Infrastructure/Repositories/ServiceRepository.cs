using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Helpers;
using Infrastructure.Helpers.Constants;
using System.Data;

namespace Infrastructure.Repositories;

public class ServiceRepository(IDbConnection connection) : BaseRepository<Service>(connection, "Services"), IServiceRepository
{
    private readonly IDbConnection _connection = connection;

    public async Task<Service?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default) =>
        (await _connection.QueryAsync<Service, ServiceCategory, Specialization, Service>(
            ServiceSqlBuilder.GetByIdWithDependencies(),
            (s, c, sp) => { s.Category = c; s.Specialization = sp; return s; },
            new { Id = id },
            splitOn: DapperConstants.SplitOnDoubleId)).FirstOrDefault();

    public async Task<IReadOnlyCollection<Service>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default) =>
        (await _connection.QueryAsync<Service, ServiceCategory, Specialization, Service>(
            ServiceSqlBuilder.GetAllWithDependencies(),
            (s, c, sp) => { s.Category = c; s.Specialization = sp; return s; },
            splitOn: DapperConstants.SplitOnDoubleId)).ToList();

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _connection.ExecuteAsync(ServiceSqlBuilder.Delete(), new { Id = id });
}