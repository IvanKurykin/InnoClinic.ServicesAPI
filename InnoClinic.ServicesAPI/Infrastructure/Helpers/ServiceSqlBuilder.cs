namespace Infrastructure.Helpers;

public static class ServiceSqlBuilder
{
    public static string GetAllWithDependencies() =>
        @"SELECT s.*, c.*, sp.*
          FROM Services s
          JOIN ServiceCategories c ON s.CategoryId = c.Id
          JOIN Specializations sp ON s.SpecializationId = sp.Id";

     public static string GetByIdWithDependencies() =>
        @"SELECT s.*, c.*, sp.*
          FROM Services s
          JOIN ServiceCategories c ON s.CategoryId = c.Id
          JOIN Specializations sp ON s.SpecializationId = sp.Id
          WHERE s.Id = @Id";

    public static string Delete() =>
        "DELETE FROM Services WHERE Id = @Id;";
}