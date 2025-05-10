namespace Infrastructure.Helpers;

public class SpecializationSqlBuilder
{
    public static string GetAllWithServices() =>
        @"SELECT sp.*, s.*
          FROM Specializations sp
          LEFT JOIN Services s ON s.SpecializationId = sp.Id";

    public static string GetByIdWithServices() =>
        @"SELECT sp.*, s.*
          FROM Specializations sp
          LEFT JOIN Services s ON s.SpecializationId = sp.Id
          WHERE sp.Id = @Id";

    public static string DeleteWithServices() =>
        @"DELETE FROM Services WHERE SpecializationId = @Id;
          DELETE FROM Specializations WHERE Id = @Id;";
}