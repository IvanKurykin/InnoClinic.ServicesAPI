using API.Controllers;
using Application.DTO.Service;
using Application.Interfaces;
using FluentAssertions;
using Infrastructure.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Controllers;

public class ServiceControllerTests
{
    private readonly Mock<IServiceService> _serviceServiceMock;
    private readonly ServiceController _controller;

    public ServiceControllerTests()
    {
        _serviceServiceMock = new();
        _controller = new ServiceController(_serviceServiceMock.Object);
    }

    [Fact]
    public async Task CreateServiceAsyncReturnsOk()
    {
        var dto = new ServiceCreateRequestDto();
        var expected = new ServiceResponseDto();
        _serviceServiceMock.Setup(s => s.CreateAsync(dto, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.CreateServiceAsync(dto, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task UpdateServiceAsyncReturnsOk()
    {
        var dto = new ServiceUpdateRequestDto();
        var id = Guid.NewGuid();
        var expected = new ServiceResponseDto();
        _serviceServiceMock.Setup(s => s.UpdateAsync(dto, id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.UpdateServiceAsync(dto, id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task DeleteServiceAsyncReturnsOk()
    {
        var id = Guid.NewGuid();
        _serviceServiceMock.Setup(s => s.DeleteAsync(id, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _controller.DeleteServiceAsync(id, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(ControllerMessages.ServiceDeletedSuccessfullyMessage);
    }

    [Fact]
    public async Task GetServiceByIdAsyncReturnsDto()
    {
        var id = Guid.NewGuid();
        var expected = new ServiceResponseDto();
        _serviceServiceMock.Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetServiceByIdAsync(id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetServicesAsyncReturnsList()
    {
        var expected = new List<ServiceResponseDto>();
        _serviceServiceMock.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetServicesAsync(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetServicesWithDependenciesAsyncReturnsList()
    {
        var expected = new List<ServiceResponseDto>();
        _serviceServiceMock.Setup(s => s.GetAllWithDependenciesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetServicesWithDependenciesAsync(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetServiceByIdWithDependenciesAsyncReturnsDto()
    {
        var id = Guid.NewGuid();
        var expected = new ServiceResponseDto();
        _serviceServiceMock.Setup(s => s.GetWithDependenciesAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetServiceByIdWithDependenciesAsync(id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }
}