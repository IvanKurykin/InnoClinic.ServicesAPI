namespace UnitTests.TestCases;

public static class ServiceCategoryRepositoryTestConstants
{
    public static readonly string ConnectionString = Environment.GetEnvironmentVariable("TEST_DB_CONNECTION") ?? "Server=(localdb)\\mssqllocaldb;Integrated Security=true;";
    public const string DatabaseName = "ServiceCategoryTestDb";
    public const string CheckDbExistsSql = "IF DB_ID('ServiceCategoryTestDb') IS NOT NULL DROP DATABASE ServiceCategoryTestDb;";
    public const string CreateDbSql = "CREATE DATABASE ServiceCategoryTestDb;";
    public const string UseDbSql = "USE ServiceCategoryTestDb;";
    public const string CreateTablesSql = @"
        CREATE TABLE ServiceCategories (
            Id UNIQUEIDENTIFIER PRIMARY KEY,
            Name NVARCHAR(100),
            TimeSlotDurationInMinutes INT
        );

        CREATE TABLE Services (
            Id UNIQUEIDENTIFIER PRIMARY KEY,
            Name NVARCHAR(100),
            Price DECIMAL(18, 2),
            Status INT,
            CategoryId UNIQUEIDENTIFIER,
            SpecializationId UNIQUEIDENTIFIER,
            FOREIGN KEY (CategoryId) REFERENCES ServiceCategories(Id) );";
    public const string InsertCategorySql = "INSERT INTO ServiceCategories (Id, Name, TimeSlotDurationInMinutes) VALUES (@Id, @Name, @Duration)";
    public const string InsertServiceSql = @"INSERT INTO Services (Id, Name, Price, Status, CategoryId, SpecializationId) VALUES (@Id, @Name, @Price, @Status, @CategoryId, @SpecializationId)";
    public const string CloseConnectionsSql = "ALTER DATABASE ServiceCategoryTestDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
    public const string DropDbSql = "DROP DATABASE ServiceCategoryTestDb;";
}