using API.Controllers;
using Application.DTO.Specialization;
using Application.Interfaces;
using FluentAssertions;
using Infrastructure.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Controllers;

public class SpecializationControllerTests
{
    private readonly Mock<ISpecializationService> _specializationServiceMock;
    private readonly SpecializationController _controller;

    public SpecializationControllerTests()
    {
        _specializationServiceMock = new();
        _controller = new SpecializationController(_specializationServiceMock.Object);
    }

    [Fact]
    public async Task CreateSpecializationAsyncReturnsOk()
    {
        var dto = new SpecializationCreateRequestDto();
        var expected = new SpecializationResponseDto();
        _specializationServiceMock.Setup(s => s.CreateAsync(dto, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.CreateSpecializationAsync(dto, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task UpdateSpecializationAsyncReturnsOk()
    {
        var dto = new SpecializationUpdateRequestDto();
        var id = Guid.NewGuid();
        var expected = new SpecializationResponseDto();
        _specializationServiceMock.Setup(s => s.UpdateAsync(dto, id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.UpdateSpecializationAsync(dto, id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task DeleteSpecializationAsyncReturnsOk()
    {
        var id = Guid.NewGuid();
        _specializationServiceMock.Setup(s => s.DeleteAsync(id, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _controller.DeleteSpecializationAsync(id, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(ControllerMessages.SpecializationDeletedSuccessfullyMessage);
    }

    [Fact]
    public async Task GetSpecializationByIdAsyncReturnsDto()
    {
        var id = Guid.NewGuid();
        var expected = new SpecializationResponseDto();
        _specializationServiceMock.Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetSpecializationByIdAsync(id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetSpecializationsAsyncReturnsList()
    {
        var expected = new List<SpecializationResponseDto>();
        _specializationServiceMock.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetSpecializationsAsync(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetSpecializationsWithDependenciesAsyncReturnsList()
    {
        var expected = new List<SpecializationResponseDto>();
        _specializationServiceMock.Setup(s => s.GetAllWithDependenciesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetSpecializationsWithDependenciesAsync(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetSpecializationByIdWithDependenciesAsyncReturnsDto()
    {
        var id = Guid.NewGuid();
        var expected = new SpecializationResponseDto();
        _specializationServiceMock.Setup(s => s.GetWithDependenciesAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _controller.GetSpecializationByIdWithDependenciesAsync(id, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }
}