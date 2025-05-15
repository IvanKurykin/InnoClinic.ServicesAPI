using Microsoft.Extensions.Configuration;

namespace UnitTests.TestCases;

public static class TestConfiguration
{
    public static IConfigurationRoot Configuration { get; }

    static TestConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();
    }

    public static string? ConnectionString => Environment.GetEnvironmentVariable("TEST_DB_CONNECTION_STRING")
        ?? Configuration.GetConnectionString("TestDb");
}
