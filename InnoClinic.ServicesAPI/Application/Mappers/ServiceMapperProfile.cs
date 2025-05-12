using System.Diagnostics.CodeAnalysis;
using Application.DTO.Service;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

[ExcludeFromCodeCoverage]
public class ServiceMapperProfile : Profile
{
    public ServiceMapperProfile()
    {
        CreateMap<ServiceRequestDto, Service>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Specialization, opt => opt.Ignore());

        CreateMap<Service, ServiceResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}