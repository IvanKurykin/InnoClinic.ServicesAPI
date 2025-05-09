using System.Diagnostics.CodeAnalysis;
using Application.DTO.ServiceCategory;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;


[ExcludeFromCodeCoverage]
public class ServiceCategoryMapperProfile : Profile
{
    public ServiceCategoryMapperProfile()
    {
        CreateMap<ServiceCategoryRequestDto, ServiceCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Services, opt => opt.Ignore());

        CreateMap<ServiceCategory, ServiceCategoryResponseDto>();
    }
}