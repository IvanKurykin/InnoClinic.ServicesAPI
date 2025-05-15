using API.Controllers;
using Application.DTO.ServiceCategory;
using Application.Interfaces;
using FluentAssertions;
using Infrastructure.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Controllers;

public class ServiceCategoryControllerTests
{
    private readonly Mock<IServiceCategoryService> _serviceCategoryServiceMock;
    private readonly ServiceCategoryController _controller;

    public ServiceCategoryControllerTests()
    {
        _serviceCategoryServiceMock = new();
        _controller = new ServiceCategoryController(_serviceCategoryServiceMock.Object);
    }

    [Fact]
    public async Task CreateServiceCategoryAsyncReturnsOk()
    {
        var dto = new ServiceCategoryCreateRequestDto();
        var expected = new ServiceCategoryResponseDto();
        _serviceCategoryServiceMock.Setup(s => s.CreateAsync(dto, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.CreateServiceCategoryAsync(dto, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task UpdateServiceCategoryAsyncReturnsOk()
    {
        var dto = new ServiceCategoryUpdateRequestDto();
        var id = Guid.NewGuid();
        var expected = new ServiceCategoryResponseDto();
        _serviceCategoryServiceMock.Setup(s => s.UpdateAsync(dto, id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.UpdateServiceCategoryAsync(dto, id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task DeleteServiceCategoryAsyncReturnsOk()
    {
        var id = Guid.NewGuid();
        _serviceCategoryServiceMock.Setup(s => s.DeleteAsync(id, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _controller.DeleteServiceCategoryAsync(id, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(ControllerMessages.ServiceCategoryDeletedSuccessfullyMessage);
    }

    [Fact]
    public async Task GetServiceCategoryByIdAsyncReturnsDto()
    {
        var id = Guid.NewGuid();
        var expected = new ServiceCategoryResponseDto();
        _serviceCategoryServiceMock.Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetServiceCategoryByIdAsync(id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetServiceCategoriesAsyncReturnsList()
    {
        var expected = new List<ServiceCategoryResponseDto>();
        _serviceCategoryServiceMock.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetServiceCategoriesAsync(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetServiceCategoriesWithDependenciesAsyncReturnsList()
    {
        var expected = new List<ServiceCategoryResponseDto>();
        _serviceCategoryServiceMock.Setup(s => s.GetAllWithDependenciesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetServiceCategoriesWithDependenciesAsync(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetServiceCategoryByIdWithDependenciesAsyncReturnsDto()
    {
        var id = Guid.NewGuid();
        var expected = new ServiceCategoryResponseDto();
        _serviceCategoryServiceMock.Setup(s => s.GetWithDependenciesAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetServiceCategoryByIdWithDependenciesAsync(id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }
}