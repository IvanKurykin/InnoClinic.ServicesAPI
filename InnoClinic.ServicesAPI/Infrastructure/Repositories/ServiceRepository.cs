using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Helpers;
using System.Data;
namespace Infrastructure.Repositories;

public class ServiceRepository(IDbConnection connection) : BaseRepository<Service>(connection, "Services"), IServiceRepository
{
    private readonly IDbConnection _connection = connection;

    public async Task<Service?> GetWithDependenciesAsync(Guid id) =>
        (await _connection.QueryAsync<Service, ServiceCategory, Specialization, Service>(
            ServiceSqlBuilder.GetByIdWithDependencies(),
            (s, c, sp) => { s.Category = c; s.Specialization = sp; return s; },
            new { Id = id },
            splitOn: DapperConstants.SplitOnDoubleId)).FirstOrDefault();

    public async Task<IList<Service>> GetAllWithDependenciesAsync() =>
        (await _connection.QueryAsync<Service, ServiceCategory, Specialization, Service>(
            ServiceSqlBuilder.GetAllWithDependencies(),
            (s, c, sp) => { s.Category = c; s.Specialization = sp; return s; },
            splitOn: DapperConstants.SplitOnDoubleId)).ToList();

    public async Task DeleteCustomAsync(Guid id) =>
        await _connection.ExecuteAsync(ServiceSqlBuilder.Delete(), new { Id = id });
}