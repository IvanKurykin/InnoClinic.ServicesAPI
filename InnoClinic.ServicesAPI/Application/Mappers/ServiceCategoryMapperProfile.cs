using System.Diagnostics.CodeAnalysis;
using Application.DTO.Service;
using Application.DTO.ServiceCategory;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

[ExcludeFromCodeCoverage]
public class ServiceCategoryMapperProfile : Profile
{
    public ServiceCategoryMapperProfile()
    {
        CreateMap<ServiceCategoryCreateRequestDto, ServiceCategory>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Services, opt => opt.Ignore());

        CreateMap<ServiceCategoryUpdateRequestDto, ServiceCategory>()
            .ForMember(dest => dest.Services, opt => opt.Ignore());

        CreateMap<ServiceCategory, ServiceCategoryResponseDto>()
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services == null ? Enumerable.Empty<ServiceResponseDto>() : src.Services.Select(s => new ServiceResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Status = s.Status,
                SpecializationId = s.SpecializationId,
                CategoryId = s.CategoryId,
            }).ToList().AsReadOnly()));
    }
}