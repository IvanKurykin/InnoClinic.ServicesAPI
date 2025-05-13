using Application.DTO.ServiceCategory;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Services;
using Moq;
using UnitTests.TestCases;

namespace UnitTests.Services;

public class ServiceCategoryServiceTests
{
    private readonly Mock<IServiceCategoryRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ServiceCategoryService _service;

    public ServiceCategoryServiceTests()
    {
        _mockRepository = new Mock<IServiceCategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new ServiceCategoryService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsyncShouldReturnCategoryResponseDto()
    {
        _mockMapper.Setup(m => m.Map<ServiceCategory>(It.IsAny<ServiceCategoryCreateRequestDto>())).Returns(ServiceCategoryTestCases.ServiceCategory);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<ServiceCategory>(), It.IsAny<CancellationToken>())).ReturnsAsync(ServiceCategoryTestCases.ServiceCategory);
        _mockMapper.Setup(m => m.Map<ServiceCategoryResponseDto>(It.IsAny<ServiceCategory>())).Returns(ServiceCategoryTestCases.ResponseDto);

        var result = await _service.CreateAsync(ServiceCategoryTestCases.CreateRequestDto);

        result.Should().NotBeNull();
        result.Name.Should().Be(ServiceCategoryTestCases.ServiceCategory.Name);
    }

    [Fact]
    public async Task GetByIdAsyncShouldReturnCategoryResponseDto()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(ServiceCategoryTestCases.ServiceCategory);
        _mockMapper.Setup(m => m.Map<ServiceCategoryResponseDto>(It.IsAny<ServiceCategory>())).Returns(ServiceCategoryTestCases.ResponseDto);

        var result = await _service.GetByIdAsync(ServiceCategoryTestCases.ServiceCategory.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be(ServiceCategoryTestCases.ServiceCategory.Name);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnAllCategories()
    {
        var list = new List<ServiceCategory> { ServiceCategoryTestCases.ServiceCategory };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);
        _mockMapper.Setup(m => m.Map<IReadOnlyCollection<ServiceCategoryResponseDto>>(It.IsAny<IReadOnlyCollection<ServiceCategory>>())).Returns(new List<ServiceCategoryResponseDto> { ServiceCategoryTestCases.ResponseDto });

        var result = await _service.GetAllAsync();

        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Name.Should().Be(ServiceCategoryTestCases.ServiceCategory.Name);
    }

    [Fact]
    public async Task UpdateAsyncShouldReturnUpdatedCategory()
    {
        var id = ServiceCategoryTestCases.ServiceCategory.Id;
        var updatedDto = new ServiceCategoryUpdateRequestDto { Name = "UpdatedName" };
        var updatedEntity = new ServiceCategory { Id = id, Name = updatedDto.Name };

        _mockRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(ServiceCategoryTestCases.ServiceCategory);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<ServiceCategory>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedEntity);
        _mockMapper.Setup(m => m.Map<ServiceCategoryResponseDto>(updatedEntity)).Returns(ServiceCategoryTestCases.ResponseDto);

        var result = await _service.UpdateAsync(updatedDto, id);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceCategory>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsyncShouldCallDeleteOnce()
    {
        var id = ServiceCategoryTestCases.ServiceCategory.Id;

        _mockRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(ServiceCategoryTestCases.ServiceCategory);
        _mockRepository.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.DeleteAsync(id);

        _mockRepository.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetWithDependenciesAsyncShouldReturnCategory()
    {
        var id = ServiceCategoryTestCases.ServiceCategory.Id;

        _mockRepository.Setup(r => r.GetWithDependenciesAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(ServiceCategoryTestCases.ServiceCategory);
        _mockMapper.Setup(m => m.Map<ServiceCategoryResponseDto>(It.IsAny<ServiceCategory>())).Returns(ServiceCategoryTestCases.ResponseDto);

        var result = await _service.GetWithDependenciesAsync(id);

        result.Should().NotBeNull();
        result.Name.Should().Be(ServiceCategoryTestCases.ResponseDto.Name);
    }

    [Fact]
    public async Task GetAllWithDependenciesAsyncShouldReturnAllCategories()
    {
        var list = new List<ServiceCategory> { ServiceCategoryTestCases.ServiceCategory };

        _mockRepository.Setup(r => r.GetAllWithDependenciesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);
        _mockMapper.Setup(m => m.Map<IReadOnlyCollection<ServiceCategoryResponseDto>>(list)).Returns(new List<ServiceCategoryResponseDto> { ServiceCategoryTestCases.ResponseDto });

        var result = await _service.GetAllWithDependenciesAsync();

        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Name.Should().Be(ServiceCategoryTestCases.ResponseDto.Name);
    }
}