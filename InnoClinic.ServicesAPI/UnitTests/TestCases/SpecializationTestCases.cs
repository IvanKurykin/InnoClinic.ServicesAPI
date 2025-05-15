using Application.DTO.Service;
using Application.DTO.Specialization;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.TestCases;

public static class SpecializationTestCases
{
    public static readonly SpecializationCreateRequestDto CreateRequestDto = new SpecializationCreateRequestDto
    {
        Name = "SpecializationName",
        Status = Statuses.Active
    };

    public static readonly Specialization Specialization = new Specialization
    {
        Id = Guid.NewGuid(),
        Name = "SpecializationName",
        Status = Statuses.Active
    };

    public static readonly SpecializationResponseDto ResponseDto = new SpecializationResponseDto
    {
        Id = Guid.NewGuid(),
        Name = "SpecializationName",
        Status = Statuses.Active,
        Services = new List<ServiceResponseDto>
            {
                new ServiceResponseDto
                {
                    Id = Guid.NewGuid(),
                    Name = "ServiceName",
                    Price = 100.50M,
                    Status = Statuses.Active
                }
            }
    };

    public static readonly SpecializationUpdateRequestDto UpdateRequestDto = new SpecializationUpdateRequestDto
    {
        Name = "UpdatedSpecializationName",
        Status = Statuses.Inactive
    };

    public static readonly Specialization UpdatedEntity = new Specialization
    {
        Id = Guid.NewGuid(),
        Name = "UpdatedSpecializationName",
        Status = Statuses.Inactive
    };
}