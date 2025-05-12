using System.Data;
using Dapper;
using Domain.Interfaces;
using Infrastructure.Helpers.Builders;

namespace Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly IDbConnection _connection;
    protected readonly string TableName;

    protected BaseRepository(IDbConnection connection, string tableName)
    {
        _connection = connection;
        TableName = tableName;
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var sql = SqlQueryBuilder.BuildInsertQuery<T>(TableName);
        await _connection.ExecuteAsync(sql, entity);
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var sql = SqlQueryBuilder.BuildUpdateQuery<T>(TableName);

        return await _connection.QuerySingleAsync<T>(sql, entity);   
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = SqlQueryBuilder.BuildSelectAllQuery(TableName);
        return (await _connection.QueryAsync<T>(sql)).ToList();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sql = SqlQueryBuilder.BuildSelectByIdQuery(TableName);
        return await _connection.QueryFirstOrDefaultAsync<T>(sql, new { id });
    }
}