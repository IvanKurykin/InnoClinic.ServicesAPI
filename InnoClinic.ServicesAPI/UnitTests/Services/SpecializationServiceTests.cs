using Application.DTO.Specialization;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Services;
using Moq;
using UnitTests.TestCases;

namespace UnitTests.Services;

public class SpecializationServiceTests
{
    private readonly Mock<ISpecializationRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ISpecializationService _service;

    public SpecializationServiceTests()
    {
        _mockRepository = new Mock<ISpecializationRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new SpecializationService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsyncShouldReturnResponseDto()
    {
        _mockMapper.Setup(m => m.Map<Specialization>(It.IsAny<SpecializationCreateRequestDto>())).Returns(SpecializationTestCases.Specialization);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Specialization>(), It.IsAny<CancellationToken>())).ReturnsAsync(SpecializationTestCases.Specialization);
        _mockMapper.Setup(m => m.Map<SpecializationResponseDto>(It.IsAny<Specialization>())).Returns(SpecializationTestCases.ResponseDto);

        var result = await _service.CreateAsync(SpecializationTestCases.CreateRequestDto);

        result.Should().NotBeNull();
        result.Name.Should().Be(SpecializationTestCases.Specialization.Name);
    }

    [Fact]
    public async Task GetByIdAsyncShouldReturnResponseDto()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(SpecializationTestCases.Specialization);
        _mockMapper.Setup(m => m.Map<SpecializationResponseDto>(It.IsAny<Specialization>())).Returns(SpecializationTestCases.ResponseDto);

        var result = await _service.GetByIdAsync(SpecializationTestCases.Specialization.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be(SpecializationTestCases.Specialization.Name);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnAll()
    {
        var list = new List<Specialization> { SpecializationTestCases.Specialization };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);
        _mockMapper.Setup(m => m.Map<IReadOnlyCollection<SpecializationResponseDto>>(It.IsAny<IReadOnlyCollection<Specialization>>())).Returns(new List<SpecializationResponseDto> { SpecializationTestCases.ResponseDto });

        var result = await _service.GetAllAsync();

        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Name.Should().Be(SpecializationTestCases.Specialization.Name);
    }

    [Fact]
    public async Task UpdateAsyncShouldReturnUpdated()
    {
        var id = SpecializationTestCases.Specialization.Id;
        var updateDto = new SpecializationUpdateRequestDto { Name = "Updated" };
        var updatedEntity = new Specialization { Id = id, Name = updateDto.Name };

        _mockRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(SpecializationTestCases.Specialization);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Specialization>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedEntity);

        var result = await _service.UpdateAsync(updateDto, id);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Specialization>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsyncShouldCallDeleteOnce()
    {
        var id = SpecializationTestCases.Specialization.Id;

        _mockRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(SpecializationTestCases.Specialization);
        _mockRepository.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.DeleteAsync(id);

        _mockRepository.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetWithDependenciesAsyncShouldReturnDto()
    {
        var id = SpecializationTestCases.Specialization.Id;

        _mockRepository.Setup(r => r.GetWithDependenciesAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(SpecializationTestCases.Specialization);
        _mockMapper.Setup(m => m.Map<SpecializationResponseDto>(It.IsAny<Specialization>())).Returns(SpecializationTestCases.ResponseDto);

        var result = await _service.GetWithDependenciesAsync(id);

        result.Should().NotBeNull();
        result.Name.Should().Be(SpecializationTestCases.ResponseDto.Name);
    }

    [Fact]
    public async Task GetAllWithDependenciesAsyncShouldReturnAll()
    {
        var list = new List<Specialization> { SpecializationTestCases.Specialization };

        _mockRepository.Setup(r => r.GetAllWithDependenciesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);
        _mockMapper.Setup(m => m.Map<IReadOnlyCollection<SpecializationResponseDto>>(list)).Returns(new List<SpecializationResponseDto> { SpecializationTestCases.ResponseDto });

        var result = await _service.GetAllWithDependenciesAsync();

        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Name.Should().Be(SpecializationTestCases.ResponseDto.Name);
    }
}
