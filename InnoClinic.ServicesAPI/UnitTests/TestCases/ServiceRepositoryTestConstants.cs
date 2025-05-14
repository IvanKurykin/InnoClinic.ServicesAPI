namespace UnitTests.TestCases;

public static class ServiceRepositoryTestConstants
{
    public static readonly string ConnectionString = Environment.GetEnvironmentVariable("TEST_DB_CONNECTION") ?? "Server=(localdb)\\mssqllocaldb;Integrated Security=true;"; 
    public const string DatabaseName = "ServiceTestDb";
    public const string CheckDbExistsSql = "IF DB_ID('ServiceTestDb') IS NOT NULL DROP DATABASE ServiceTestDb;";
    public const string CreateDbSql = "CREATE DATABASE ServiceTestDb;";
    public const string UseDbSql = "USE ServiceTestDb;";
    public const string CreateTablesSql = @"
            CREATE TABLE ServiceCategories (
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                Name NVARCHAR(100),
                TimeSlotDurationInMinutes INT
            );

            CREATE TABLE Specializations (
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                Name NVARCHAR(100),
                Status INT
            );

            CREATE TABLE Services (
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                Name NVARCHAR(100),
                Price DECIMAL(18, 2),
                Status INT,
                CategoryId UNIQUEIDENTIFIER,
                SpecializationId UNIQUEIDENTIFIER,
                FOREIGN KEY (CategoryId) REFERENCES ServiceCategories(Id),
                FOREIGN KEY (SpecializationId) REFERENCES Specializations(Id)
            );";

    public const string InsertCategorySql = "INSERT INTO ServiceCategories (Id, Name, TimeSlotDurationInMinutes) VALUES (@Id, @Name, @Duration)";
    public const string InsertSpecializationSql = "INSERT INTO Specializations (Id, Name, Status) VALUES (@Id, @Name, @Status)";
    public const string InsertServiceSql = @"INSERT INTO Services (Id, Name, Price, Status, CategoryId, SpecializationId) VALUES (@Id, @Name, @Price, @Status, @CategoryId, @SpecializationId)";
    public const string CloseConnectionsSql = "ALTER DATABASE ServiceTestDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
    public const string DropDbSql = "DROP DATABASE ServiceTestDb;";
}