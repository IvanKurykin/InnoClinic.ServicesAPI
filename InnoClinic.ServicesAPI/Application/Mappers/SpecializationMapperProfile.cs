using Application.DTO.Service;
using Application.DTO.Specialization;
using AutoMapper;
using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Application.Mappers;

[ExcludeFromCodeCoverage]
public class SpecializationMapperProfile : Profile
{
    public SpecializationMapperProfile()
    {
        CreateMap<SpecializationCreateRequestDto, Specialization>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Services, opt => opt.Ignore());

        CreateMap<SpecializationUpdateRequestDto, Specialization>()
            .ForMember(dest => dest.Services, opt => opt.Ignore());

        CreateMap<Specialization, SpecializationResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
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