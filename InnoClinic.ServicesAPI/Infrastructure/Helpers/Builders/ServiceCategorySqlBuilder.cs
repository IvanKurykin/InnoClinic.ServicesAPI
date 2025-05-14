namespace Infrastructure.Helpers.Builders;

public static class ServiceCategorySqlBuilder
{
    public const string GetAllWithDependencies =
       @"SELECT c.*, s.*
          FROM ServiceCategories c
          LEFT JOIN Services s ON s.CategoryId = c.Id";

    public const string GetByIdWithDependencies =
        @"SELECT c.*, s.*
          FROM ServiceCategories c
          LEFT JOIN Services s ON s.CategoryId = c.Id
          WHERE c.Id = @Id";

    public const string Delete =
        @"DELETE FROM Services WHERE CategoryId = @Id;
          DELETE FROM ServiceCategories WHERE Id = @Id;";
}