using Application.DTO.Service;
using Application.DTO.ServiceCategory;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.TestCases;

public static class ServiceCategoryTestCases
{
    public static readonly ServiceCategoryCreateRequestDto CreateRequestDto = new ServiceCategoryCreateRequestDto
    {
        Name = "CategoryName",
        TimeSlotDurationInMinutes = 30
    };

    public static readonly ServiceCategory ServiceCategory = new ServiceCategory
    {
        Id = Guid.NewGuid(),
        Name = "CategoryName",
        TimeSlotDurationInMinutes = 30
    };

    public static readonly ServiceCategoryResponseDto ResponseDto = new ServiceCategoryResponseDto
    {
        Id = Guid.NewGuid(),
        Name = "CategoryName",
        TimeSlotDurationInMinutes = 30,
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

    public static readonly ServiceCategoryUpdateRequestDto UpdateRequestDto = new ServiceCategoryUpdateRequestDto
    {
        Name = "UpdatedCategoryName",
        TimeSlotDurationInMinutes = 45
    };

    public static readonly ServiceCategory UpdatedEntity = new ServiceCategory
    {
        Id = Guid.NewGuid(),
        Name = "UpdatedCategoryName",
        TimeSlotDurationInMinutes = 45
    };
}
