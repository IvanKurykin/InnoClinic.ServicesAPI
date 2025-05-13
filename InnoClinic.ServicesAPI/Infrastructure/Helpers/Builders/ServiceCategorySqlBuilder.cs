namespace Infrastructure.Helpers.Builders;

public static class ServiceCategorySqlBuilder
{
    public static string GetAllWithDependencies() =>
       @"SELECT c.*, s.*
          FROM ServiceCategories c
          LEFT JOIN Services s ON s.CategoryId = c.Id";

    public static string GetByIdWithDependencies() =>
        @"SELECT c.*, s.*
          FROM ServiceCategories c
          LEFT JOIN Services s ON s.CategoryId = c.Id
          WHERE c.Id = @Id";

    public static string Delete() =>
        @"DELETE FROM Services WHERE CategoryId = @Id;
          DELETE FROM ServiceCategories WHERE Id = @Id;";
}