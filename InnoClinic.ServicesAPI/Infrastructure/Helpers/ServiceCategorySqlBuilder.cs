namespace Infrastructure.Helpers;

public class ServiceCategorySqlBuilder
{
    public static string GetAllWithServices() =>
       @"SELECT c.*, s.*
          FROM ServiceCategories c
          LEFT JOIN Services s ON s.CategoryId = c.Id";

    public static string GetByIdWithServices() =>
        @"SELECT c.*, s.*
          FROM ServiceCategories c
          LEFT JOIN Services s ON s.CategoryId = c.Id
          WHERE c.Id = @Id";

    public static string DeleteWithServices() =>
        @"DELETE FROM Services WHERE CategoryId = @Id;
          DELETE FROM ServiceCategories WHERE Id = @Id;";
}