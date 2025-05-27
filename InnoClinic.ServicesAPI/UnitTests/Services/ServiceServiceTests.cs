using Application.DTO.Service;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Services;
using InnoClinic.Messaging.Abstractions;
using Moq;
using UnitTests.TestCases;

namespace UnitTests.Services;

public class ServiceServiceTests
{
    private readonly Mock<IServiceRepository> _mockServiceRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IMessagePublisher> _mockMessagePublisher;
    private readonly ServiceService _service;

    public ServiceServiceTests()
    {
        _mockServiceRepository = new Mock<IServiceRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockMessagePublisher = new Mock<IMessagePublisher>();
        _service = new ServiceService(_mockServiceRepository.Object, _mockMapper.Object, _mockMessagePublisher.Object);
    }

    [Fact]
    public async Task CreateAsyncShouldReturnServiceResponseDto()
    {
        _mockMapper.Setup(m => m.Map<Service>(It.IsAny<ServiceCreateRequestDto>())).Returns(ServiceTestCases.Service);
        _mockServiceRepository.Setup(r => r.CreateAsync(It.IsAny<Service>(), It.IsAny<CancellationToken>())).ReturnsAsync(ServiceTestCases.Service);
        _mockMapper.Setup(m => m.Map<ServiceResponseDto>(It.IsAny<Service>())).Returns(ServiceTestCases.ResponseDto);

        var result = await _service.CreateAsync(ServiceTestCases.CreateRequestDto);

        result.Should().NotBeNull();
        result.Name.Should().Be(ServiceTestCases.Service.Name);
        result.Price.Should().Be(ServiceTestCases.Service.Price);
    }

    [Fact]
    public async Task GetByIdAsyncShouldReturnServiceResponseDto()
    {
        _mockServiceRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(ServiceTestCases.Service);
        _mockMapper.Setup(m => m.Map<ServiceResponseDto>(It.IsAny<Service>())).Returns(ServiceTestCases.ResponseDto);

        var result = await _service.GetByIdAsync(ServiceTestCases.Service.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be(ServiceTestCases.Service.Name);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnAllServices()
    {
        var services = new List<Service> { ServiceTestCases.Service };
        _mockServiceRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(services);
        _mockMapper.Setup(m => m.Map<IReadOnlyCollection<ServiceResponseDto>>(It.IsAny<IReadOnlyCollection<Service>>())).Returns(new List<ServiceResponseDto> { ServiceTestCases.ResponseDto });

        var result = await _service.GetAllAsync();

        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Name.Should().Be(ServiceTestCases.Service.Name);
    }

    [Fact]
    public async Task UpdateAsyncShouldReturnUpdatedServiceResponseDto()
    {
        var serviceId = ServiceTestCases.Service.Id;
        var existingService = ServiceTestCases.Service;

        var updatedDto = new ServiceUpdateRequestDto
        {
            Name = "UpdatedServiceName",
            CategoryId = existingService.CategoryId,
            SpecializationId = existingService.SpecializationId,
            Price = existingService.Price,
            Status = existingService.Status
        };

        var updatedService = new Service
        {
            Id = serviceId,
            Name = updatedDto.Name,
            CategoryId = updatedDto.CategoryId,
            SpecializationId = updatedDto.SpecializationId,
            Price = updatedDto.Price,
            Status = updatedDto.Status
        };

        _mockServiceRepository.Setup(r => r.GetByIdAsync(serviceId, It.IsAny<CancellationToken>())).ReturnsAsync(existingService);
        _mockServiceRepository.Setup(r => r.UpdateAsync(It.IsAny<Service>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedService);
        _mockMessagePublisher.Setup(p => p.PublishEntityUpdated(updatedService, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(updatedDto, serviceId);

        _mockServiceRepository.Verify(r => r.UpdateAsync(It.IsAny<Service>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockMessagePublisher.Verify(p => p.PublishEntityUpdated(updatedService, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsyncShouldCallDeleteOnce()
    {
        var serviceId = ServiceTestCases.Service.Id;
        var service = ServiceTestCases.Service;

        _mockServiceRepository.Setup(r => r.GetByIdAsync(serviceId, It.IsAny<CancellationToken>())).ReturnsAsync(service);

        _mockServiceRepository.Setup(r => r.DeleteAsync(serviceId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.DeleteAsync(serviceId);

        _mockServiceRepository.Verify(r => r.DeleteAsync(serviceId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetWithDependenciesAsyncShouldReturnServiceResponseDto()
    {
        var serviceId = ServiceTestCases.Service.Id;
        _mockServiceRepository.Setup(r => r.GetWithDependenciesAsync(serviceId, It.IsAny<CancellationToken>())).ReturnsAsync(ServiceTestCases.Service);

        _mockMapper.Setup(m => m.Map<ServiceResponseDto>(It.IsAny<Service>())).Returns(ServiceTestCases.ResponseDto);

        var result = await _service.GetWithDependenciesAsync(serviceId);

        result.Should().NotBeNull();
        result.Name.Should().Be(ServiceTestCases.ResponseDto.Name);
    }

    [Fact]
    public async Task GetAllWithDependenciesAsyncShouldReturnAllServices()
    {
        var services = new List<Service> { ServiceTestCases.Service };
        _mockServiceRepository.Setup(r => r.GetAllWithDependenciesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(services);

        _mockMapper.Setup(m => m.Map<IReadOnlyCollection<ServiceResponseDto>>(services)).Returns(new List<ServiceResponseDto> { ServiceTestCases.ResponseDto });

        var result = await _service.GetAllWithDependenciesAsync();

        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Name.Should().Be(ServiceTestCases.ResponseDto.Name);
    }
}