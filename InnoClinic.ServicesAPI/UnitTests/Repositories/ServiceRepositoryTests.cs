using System.Data;
using AutoMapper;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Moq;
using Moq.Dapper;
using UnitTests.TestCases;

namespace UnitTests.Repositories;

public class ServiceRepositoryTests
{
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly ServiceRepository _repository;
    private readonly Mock<IMapper> _mockMapper;

    public ServiceRepositoryTests()
    {
        _mockConnection = new Mock<IDbConnection>();
        _mockMapper = new Mock<IMapper>();
        _repository = new ServiceRepository(_mockConnection.Object);
    }

    [Fact]
    public async Task CreateAsyncShouldExecuteInsert()
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Price = 100.50m,
            Status = Statuses.Active
        };

        _mockConnection.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<Service>(), null, null, null)).ReturnsAsync(1);

        var result = await _repository.CreateAsync(service);

        Assert.Equal(service.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsyncShouldExecuteUpdate()
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Updated Service",
            Price = 150.75m
        };

        _mockConnection.SetupDapperAsync(c => c.QuerySingleAsync<Service>(It.IsAny<string>(), It.IsAny<Service>(), null, null, null)).ReturnsAsync(service);

        var result = await _repository.UpdateAsync(service);

        Assert.Equal("Updated Service", result.Name);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnAllServices()
    {
        var services = new List<Service>
            {
                new Service { Id = Guid.NewGuid(), Name = "Service 1" },
                new Service { Id = Guid.NewGuid(), Name = "Service 2" }
            };

        _mockConnection.SetupDapperAsync(c => c.QueryAsync<Service>(It.IsAny<string>(), null, null, null, null)).ReturnsAsync(services);

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsyncShouldReturnService()
    {
        var serviceId = Guid.NewGuid();
        var expected = new Service { Id = serviceId, Name = "Test Service" };

        _mockConnection.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<Service>(It.IsAny<string>(), It.Is<object>(o => CheckIdParameter(o, serviceId)), null, null, null)).ReturnsAsync(expected);

        var result = await _repository.GetByIdAsync(serviceId);

        Assert.Equal(serviceId, result?.Id);
    }

    [Fact]
    public async Task DeleteAsyncShouldExecuteDelete()
    {
        var serviceId = Guid.NewGuid();
        _mockConnection.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.Is<object>(o => CheckIdParameter(o, serviceId)), null, null, null)).ReturnsAsync(1);

        await _repository.DeleteAsync(serviceId);
    }

    [Fact]
    public async Task GetWithDependenciesAsyncShouldReturnServiceWithRelatedCategoryAndSpecialization()
    {
        using var connection = new SqlConnection(TestConfiguration.ConnectionString);

        await connection.OpenAsync();

        await connection.ExecuteAsync(ServiceRepositoryTestConstants.CheckDbExistsSql);
        await connection.ExecuteAsync(ServiceRepositoryTestConstants.CreateDbSql);

        var useDbSql = ServiceRepositoryTestConstants.UseDbSql;
        await connection.ExecuteAsync(useDbSql);

        var createTablesSql = ServiceRepositoryTestConstants.CreateTablesSql;
        await connection.ExecuteAsync(createTablesSql);

        var categoryId = Guid.NewGuid();
        var specializationId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertCategorySql,
            new { Id = categoryId, Name = "Therapy", Duration = 30 });

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertSpecializationSql,
            new { Id = specializationId, Name = "General", Status = 1 });

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertServiceSql,
            new
            {
                Id = serviceId,
                Name = "Massage",
                Price = 50m,
                Status = 1,
                CategoryId = categoryId,
                SpecializationId = specializationId
            });

        var repository = new ServiceRepository(connection);

        var result = await repository.GetWithDependenciesAsync(serviceId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(serviceId);
        result.Category.Should().NotBeNull();
        result.Specialization.Should().NotBeNull();
        result.Category!.Id.Should().Be(categoryId);
        result.Specialization!.Id.Should().Be(specializationId);

        await connection.ExecuteAsync(ServiceRepositoryTestConstants.CloseConnectionsSql);
        await connection.CloseAsync();

        await connection.ExecuteAsync(ServiceRepositoryTestConstants.DropDbSql);
    }

    [Fact]
    public async Task GetAllWithDependenciesAsyncShouldReturnServicesWithRelatedCategoriesAndSpecializations()
    {
        using var connection = new SqlConnection(TestConfiguration.ConnectionString);

        await connection.OpenAsync();

        await connection.ExecuteAsync(ServiceRepositoryTestConstants.CheckDbExistsSql);
        await connection.ExecuteAsync(ServiceRepositoryTestConstants.CreateDbSql);

        var useDbSql = ServiceRepositoryTestConstants.UseDbSql;
        await connection.ExecuteAsync(useDbSql);

        var createTablesSql = ServiceRepositoryTestConstants.CreateTablesSql;
        await connection.ExecuteAsync(createTablesSql);

        var categoryId1 = Guid.NewGuid();
        var categoryId2 = Guid.NewGuid();
        var specializationId1 = Guid.NewGuid();
        var specializationId2 = Guid.NewGuid();
        var serviceId1 = Guid.NewGuid();
        var serviceId2 = Guid.NewGuid();

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertCategorySql,
            new { Id = categoryId1, Name = "Therapy", Duration = 30 });

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertCategorySql,
            new { Id = categoryId2, Name = "Massage", Duration = 45 });

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertSpecializationSql,
            new { Id = specializationId1, Name = "General", Status = 1 });

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertSpecializationSql,
            new { Id = specializationId2, Name = "Advanced", Status = 1 });

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertServiceSql,
            new
            {
                Id = serviceId1,
                Name = "Service1",
                Price = 100m,
                Status = 1,
                CategoryId = categoryId1,
                SpecializationId = specializationId1
            });

        await connection.ExecuteAsync(
            ServiceRepositoryTestConstants.InsertServiceSql,
            new
            {
                Id = serviceId2,
                Name = "Service2",
                Price = 150m,
                Status = 1,
                CategoryId = categoryId2,
                SpecializationId = specializationId2
            });

        var repository = new ServiceRepository(connection);
        var result = await repository.GetAllWithDependenciesAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(s => s.Id == serviceId1 && s.Category!.Id == categoryId1 && s.Specialization!.Id == specializationId1);
        result.Should().Contain(s => s.Id == serviceId2 && s.Category!.Id == categoryId2 && s.Specialization!.Id == specializationId2);

        await connection.ExecuteAsync(ServiceRepositoryTestConstants.CloseConnectionsSql);
        await connection.CloseAsync();

        await connection.ExecuteAsync(ServiceRepositoryTestConstants.DropDbSql);
    }

    private static bool CheckIdParameter(object param, Guid expectedId)
    {
        var prop = param.GetType().GetProperty("id") ?? param.GetType().GetProperty("Id");
        if (prop is null) return false;

        var value = prop.GetValue(param);
        return value is not null && value.Equals(expectedId);
    }
}