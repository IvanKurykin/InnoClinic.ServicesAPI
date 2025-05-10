using System.Text;

namespace Infrastructure.Helpers;

public static class SqlQueryBuilder
{
    private const string Id = "Id";

    public static string BuildInsertQuery<T>(string tableName)
    {
        var properties = typeof(T).GetProperties().Where(p => p.Name is not Id).Select(p => p.Name);

        var columns = string.Join(", ", properties);
        var values = string.Join(", ", properties.Select(p => $"@{p}"));

        return new StringBuilder()
            .Append($"INSERT INTO {tableName} ({columns}) ")
            .Append($"VALUES ({values}); ")
            .Append("SELECT SCOPE_IDENTITY();")
            .ToString();
    }

    public static string BuildUpdateQuery<T>(string tableName)
    {
        var setClause = string.Join(", ",
            typeof(T).GetProperties()
                .Where(p => p.Name is not Id)
                .Select(p => $"{p.Name} = @{p.Name}"));

        return new StringBuilder()
            .Append($"UPDATE {tableName} ")
            .Append($"SET {setClause} ")
            .Append($"WHERE {Id} = @Id")
            .ToString();
    }

    public static string BuildSelectAllQuery(string tableName) =>
        $"SELECT * FROM {tableName}";

    public static string BuildSelectByIdQuery(string tableName) =>
        new StringBuilder().Append($"SELECT * FROM {tableName} ").Append($"WHERE {Id} = @id").ToString();
}