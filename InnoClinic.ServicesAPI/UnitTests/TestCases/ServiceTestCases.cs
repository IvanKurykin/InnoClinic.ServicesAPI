using Application.DTO.Service;
using Application.DTO.ServiceCategory;
using Application.DTO.Specialization;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.TestCases;

public static class ServiceTestCases
{
    public static readonly ServiceCreateRequestDto CreateRequestDto = new ServiceCreateRequestDto
    {
        CategoryId = Guid.NewGuid(),
        SpecializationId = Guid.NewGuid(),
        Name = "ServiceName",
        Price = 100.50M,
        Status = Statuses.Active
    };

    public static readonly Service Service = new Service
    {
        Id = Guid.NewGuid(),
        CategoryId = Guid.NewGuid(),
        SpecializationId = Guid.NewGuid(),
        Name = "ServiceName",
        Price = 100.50M,
        Status = Statuses.Active
    };

    public static readonly ServiceResponseDto ResponseDto = new ServiceResponseDto
    {
        Id = Guid.NewGuid(),
        Category = new ServiceCategoryResponseDto
        {
            Id = Guid.NewGuid(),
            Name = "CategoryName",
            TimeSlotDurationInMinutes = 30
        },
        Specialization = new SpecializationResponseDto
        {
            Id = Guid.NewGuid(),
            Name = "SpecializationName",
            Status = Statuses.Active
        },
        Name = "ServiceName",
        Price = 100.50M,
        Status = Statuses.Active
    };

    public static readonly ServiceUpdateRequestDto UpdateRequestDto = new ServiceUpdateRequestDto
    {
        CategoryId = Guid.NewGuid(),
        SpecializationId = Guid.NewGuid(),
        Name = "UpdatedServiceName",
        Price = 120.75M,
        Status = Statuses.Inactive
    };
}