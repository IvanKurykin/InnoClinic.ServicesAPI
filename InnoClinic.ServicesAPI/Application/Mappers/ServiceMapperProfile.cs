using System.Diagnostics.CodeAnalysis;
using Application.DTO.Service;
using Application.DTO.ServiceCategory;
using Application.DTO.Specialization;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

[ExcludeFromCodeCoverage]
public class ServiceMapperProfile : Profile
{
    public ServiceMapperProfile()
    {
        CreateMap<ServiceCreateRequestDto, Service>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Specialization, opt => opt.Ignore());

        CreateMap<ServiceUpdateRequestDto, Service>()
           .ForMember(dest => dest.Category, opt => opt.Ignore())
           .ForMember(dest => dest.Specialization, opt => opt.Ignore());

        CreateMap<Service, ServiceResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src =>
                src.Category == null ? null : new ServiceCategoryResponseDto
                {
                    Id = src.Category.Id,
                    Name = src.Category.Name,
                    TimeSlotDurationInMinutes = src.Category.TimeSlotDurationInMinutes
                    
                }))
            .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src =>
                src.Specialization == null ? null : new SpecializationResponseDto
                {
                    Id = src.Specialization.Id,
                    Name = src.Specialization.Name,
                    Status = src.Specialization.Status
                }));
    }
}