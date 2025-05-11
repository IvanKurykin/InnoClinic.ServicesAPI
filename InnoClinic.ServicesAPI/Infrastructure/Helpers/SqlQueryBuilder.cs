namespace Infrastructure.Helpers;

public static class SqlQueryBuilder
{
    private const string Id = "Id";

    public static string BuildInsertQuery<T>(string tableName)
    {
        var properties = typeof(T).GetProperties().Where(p => p.Name is not Id && p.PropertyType.IsSimpleType()).Select(p => p.Name);

        var columns = string.Join(", ", properties);
        var values = string.Join(", ", properties.Select(p => $"@{p}"));

        return $"INSERT INTO {tableName} ({columns}) VALUES ({values}); SELECT SCOPE_IDENTITY();";
    }

    public static string BuildUpdateQuery<T>(string tableName)
    {
        var setClause = string.Join(", ", typeof(T).GetProperties().Where(p => p.Name is not Id && p.PropertyType.IsSimpleType()).Select(p => $"{p.Name} = @{p.Name}"));

        return $"UPDATE {tableName} SET {setClause} WHERE {Id} = @Id";
    }

    public static string BuildSelectAllQuery(string tableName) =>
        $"SELECT * FROM {tableName}";

    public static string BuildSelectByIdQuery(string tableName) =>
        $"SELECT * FROM {tableName} WHERE {Id} = @id";

    public static bool IsSimpleType(this Type type) =>
        type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(Guid) || type.IsEnum;
}