using Dapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Helpers.Builders;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Moq;
using Moq.Dapper;
using System.Data;
using UnitTests.TestCases;

namespace UnitTests.Repositories;

public class ServiceCategoryRepositoryTests
{
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly ServiceCategoryRepository _repository;

    public ServiceCategoryRepositoryTests()
    {
        _mockConnection = new Mock<IDbConnection>();
        _repository = new ServiceCategoryRepository(_mockConnection.Object);
    }

    [Fact]
    public async Task CreateAsyncShouldExecuteInsert()
    {
        var category = new ServiceCategory { Id = Guid.NewGuid(), Name = "Test" };
        _mockConnection.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<ServiceCategory>(), null, null, null)).ReturnsAsync(1);

        var result = await _repository.CreateAsync(category);

        Assert.Equal(category.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsyncShouldExecuteUpdate()
    {
        var category = new ServiceCategory { Id = Guid.NewGuid(), Name = "Updated" };
        _mockConnection.SetupDapperAsync(c => c.QuerySingleAsync<ServiceCategory>(It.IsAny<string>(), It.IsAny<ServiceCategory>(), null, null, null)).ReturnsAsync(category);

        var result = await _repository.UpdateAsync(category);

        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnAllCategories()
    {
        var categories = new List<ServiceCategory>
            {
                new ServiceCategory { Id = Guid.NewGuid(), Name = "Category 1" },
                new ServiceCategory { Id = Guid.NewGuid(), Name = "Category 2" }
            };

        _mockConnection.SetupDapperAsync(c => c.QueryAsync<ServiceCategory>(It.IsAny<string>(), null, null, null, null)).ReturnsAsync(categories);

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsyncShouldReturnCategory()
    {
        var categoryId = Guid.NewGuid();
        var expected = new ServiceCategory { Id = categoryId, Name = "Test" };

        _mockConnection.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<ServiceCategory>(It.IsAny<string>(), It.Is<object>(o => CheckIdParameter(o, categoryId)), null, null, null)).ReturnsAsync(expected);

        var result = await _repository.GetByIdAsync(categoryId);

        Assert.Equal(categoryId, result?.Id);
    }

    [Fact]
    public async Task DeleteAsyncShouldExecuteDelete()
    {
        var categoryId = Guid.NewGuid();
        _mockConnection.SetupDapperAsync(c => c.ExecuteAsync(
            It.IsAny<string>(),
            It.Is<object>(o => CheckIdParameter(o, categoryId)), null, null, null)).ReturnsAsync(1);

        await _repository.DeleteAsync(categoryId);
    }

    [Fact]
    public async Task GetWithDependenciesAsyncShouldReturnCategoryWithRelatedServices()
    {
        using var connection = new SqlConnection(ServiceCategoryRepositoryTestConstants.ConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(ServiceCategoryRepositoryTestConstants.CheckDbExistsSql);
        await connection.ExecuteAsync(ServiceCategoryRepositoryTestConstants.CreateDbSql);

        var useDbSql = ServiceCategoryRepositoryTestConstants.UseDbSql;
        await connection.ExecuteAsync(useDbSql);

        var createTablesSql = ServiceCategoryRepositoryTestConstants.CreateTablesSql;
        await connection.ExecuteAsync(createTablesSql);

        var categoryId = Guid.NewGuid();
        var specializationId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        await connection.ExecuteAsync(
            ServiceCategoryRepositoryTestConstants.InsertCategorySql,
            new { Id = categoryId, Name = "Therapy", Duration = 30 });

        await connection.ExecuteAsync(
            ServiceCategoryRepositoryTestConstants.InsertServiceSql,
            new
            {
                Id = serviceId,
                Name = "Massage",
                Price = 50m,
                Status = 1,
                CategoryId = categoryId,
                SpecializationId = specializationId
            });

        var repository = new ServiceCategoryRepository(connection);

        var result = await repository.GetWithDependenciesAsync(categoryId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(categoryId);
        result.Services.Should().ContainSingle(s => s.Id == serviceId);

        await connection.ExecuteAsync(ServiceCategoryRepositoryTestConstants.CloseConnectionsSql);
        await connection.CloseAsync();
        await connection.ExecuteAsync(ServiceCategoryRepositoryTestConstants.DropDbSql);
    }

    [Fact]
    public async Task GetAllWithDependenciesAsyncShouldReturnCategoriesWithRelatedServices()
    {
        using var connection = new SqlConnection(ServiceCategoryRepositoryTestConstants.ConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(ServiceCategoryRepositoryTestConstants.CheckDbExistsSql);
        await connection.ExecuteAsync(ServiceCategoryRepositoryTestConstants.CreateDbSql);

        var useDbSql = ServiceCategoryRepositoryTestConstants.UseDbSql;
        await connection.ExecuteAsync(useDbSql);

        var createTablesSql = ServiceCategoryRepositoryTestConstants.CreateTablesSql;
        await connection.ExecuteAsync(createTablesSql);

        var categoryId1 = Guid.NewGuid();
        var categoryId2 = Guid.NewGuid();
        var serviceId1 = Guid.NewGuid();
        var serviceId2 = Guid.NewGuid();

        await connection.ExecuteAsync(
            ServiceCategoryRepositoryTestConstants.InsertCategorySql,
            new { Id = categoryId1, Name = "Therapy", Duration = 30 });

        await connection.ExecuteAsync(
            ServiceCategoryRepositoryTestConstants.InsertCategorySql,
            new { Id = categoryId2, Name = "Massage", Duration = 45 });

        await connection.ExecuteAsync(
            ServiceCategoryRepositoryTestConstants.InsertServiceSql,
            new
            {
                Id = serviceId1,
                Name = "Service1",
                Price = 100m,
                Status = 1,
                CategoryId = categoryId1,
                SpecializationId = Guid.NewGuid()
            });

        await connection.ExecuteAsync(
            ServiceCategoryRepositoryTestConstants.InsertServiceSql,
            new
            {
                Id = serviceId2,
                Name = "Service2",
                Price = 150m,
                Status = 1,
                CategoryId = categoryId2,
                SpecializationId = Guid.NewGuid()
            });

        var repository = new ServiceCategoryRepository(connection);
        var result = await repository.GetAllWithDependenciesAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Id == categoryId1 && c.Services.Any(s => s.Id == serviceId1));
        result.Should().Contain(c => c.Id == categoryId2 && c.Services.Any(s => s.Id == serviceId2));

        await connection.ExecuteAsync(ServiceCategoryRepositoryTestConstants.CloseConnectionsSql);
        await connection.CloseAsync();
        await connection.ExecuteAsync(ServiceCategoryRepositoryTestConstants.DropDbSql);
    }

    private static bool CheckIdParameter(object param, Guid expectedId)
    {
        var prop = param.GetType().GetProperty("id") ?? param.GetType().GetProperty("Id");
        if (prop is null) return false;

        var value = prop.GetValue(param);
        return value is not null && value.Equals(expectedId);
    }
}