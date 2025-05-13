using Dapper;
using Domain.Entities;
using Infrastructure.Repositories;
using Moq;
using Moq.Dapper;
using System.Data;

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

    private bool CheckIdParameter(object param, Guid expectedId)
    {
        var prop = param.GetType().GetProperty("id") ?? param.GetType().GetProperty("Id");
        if (prop is null) return false;

        var value = prop.GetValue(param);
        return value is not null && value.Equals(expectedId);
    }
}