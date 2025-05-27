namespace Infrastructure.Helpers.Builders;

public static class SpecializationSqlBuilder
{
    public const string GetAllWithDependencies =
        @"SELECT sp.*, s.*
          FROM Specializations sp
          LEFT JOIN Services s ON s.SpecializationId = sp.Id";

    public const string GetByIdWithDependencies =
        @"SELECT sp.*, s.*
          FROM Specializations sp
          LEFT JOIN Services s ON s.SpecializationId = sp.Id
          WHERE sp.Id = @Id";

    public const string Delete =
        @"DELETE FROM Services WHERE SpecializationId = @Id;
          DELETE FROM Specializations WHERE Id = @Id;";
}