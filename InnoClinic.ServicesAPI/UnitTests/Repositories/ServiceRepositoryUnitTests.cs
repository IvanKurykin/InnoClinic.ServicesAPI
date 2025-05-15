using System.Data;
using AutoMapper;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;
using Moq.Dapper;

namespace Tests.Repositories;

public class ServiceRepositoryUnitTests
{
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly ServiceRepository _repository;
    private readonly Mock<IMapper> _mockMapper;

    public ServiceRepositoryUnitTests()
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
    public async Task GetWithDependenciesAsyncShouldReturnServiceWithCategoryAndSpecialization()
    {
        var repoMock = new Mock<IServiceRepository>();
        var testServiceId = Guid.NewGuid();
        var testCategoryId = Guid.NewGuid();
        var testSpecializationId = Guid.NewGuid();

        repoMock.Setup(r => r.GetWithDependenciesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Service
            {
                Id = testServiceId,
                Name = "Test Service",
                Category = new ServiceCategory { Id = testCategoryId, Name = "Test Category" },
                Specialization = new Specialization { Id = testSpecializationId, Name = "Test Specialization" }
            });

        var repo = repoMock.Object;
        var id = Guid.NewGuid();
        var ct = CancellationToken.None;

        var service = await repo.GetWithDependenciesAsync(id, ct);

        service.Should().NotBeNull();
        service.Id.Should().NotBeEmpty();
        service.Category.Should().NotBeNull();
        service.Specialization.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllWithDependenciesAsyncShouldReturnServicesWithCategoriesAndSpecializations()
    {
        var repoMock = new Mock<IServiceRepository>();
        var testServiceId = Guid.NewGuid();
        var testCategoryId = Guid.NewGuid();
        var testSpecializationId = Guid.NewGuid();

        repoMock.Setup(r => r.GetAllWithDependenciesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Service>
            {
                new Service
                {
                   Id = testServiceId,
                   Name = "Test Service",
                   Category = new ServiceCategory { Id = testCategoryId, Name = "Test Category" },
                   Specialization = new Specialization { Id = testSpecializationId, Name = "Test Specialization" }
                }
            });

        var repo = repoMock.Object;
        var ct = CancellationToken.None;

        var services = await repo.GetAllWithDependenciesAsync(ct);

        services.Should().NotBeNull();
        services.Should().NotBeEmpty();
        services.All(s => s.Category != null && s.Specialization != null).Should().BeTrue();
    }

    private static bool CheckIdParameter(object param, Guid expectedId)
    {
        var prop = param.GetType().GetProperty("id") ?? param.GetType().GetProperty("Id");
        if (prop is null) return false;

        var value = prop.GetValue(param);
        return value is not null && value.Equals(expectedId);
    }
}