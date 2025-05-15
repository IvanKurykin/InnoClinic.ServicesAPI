namespace Infrastructure.Helpers.Builders;

public static class ServiceSqlBuilder
{
    public const string GetAllWithDependencies =
        @"SELECT s.*, c.*, sp.*
          FROM Services s
          JOIN ServiceCategories c ON s.CategoryId = c.Id
          JOIN Specializations sp ON s.SpecializationId = sp.Id";

    public const string GetByIdWithDependencies =
       @"SELECT s.*, c.*, sp.*
          FROM Services s
          JOIN ServiceCategories c ON s.CategoryId = c.Id
          JOIN Specializations sp ON s.SpecializationId = sp.Id
          WHERE s.Id = @Id";

    public const string Delete =
        "DELETE FROM Services WHERE Id = @Id;";
}