namespace Infrastructure.Helpers;

public class SpecializationSqlBuilder
{
    public static string GetAllWithDependencies() =>
        @"SELECT sp.*, s.*
          FROM Specializations sp
          LEFT JOIN Services s ON s.SpecializationId = sp.Id";

    public static string GetByIdWithDependencies() =>
        @"SELECT sp.*, s.*
          FROM Specializations sp
          LEFT JOIN Services s ON s.SpecializationId = sp.Id
          WHERE sp.Id = @Id";

    public static string Delete() =>
        @"DELETE FROM Services WHERE SpecializationId = @Id;
          DELETE FROM Specializations WHERE Id = @Id;";
}