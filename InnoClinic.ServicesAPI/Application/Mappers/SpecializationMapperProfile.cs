using Application.DTO.Specialization;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Application.Mappers;

[ExcludeFromCodeCoverage]
public class SpecializationMapperProfile : Profile
{
    public SpecializationMapperProfile()
    {
        CreateMap<SpecializationRequestDto, Specialization>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Services, opt => opt.Ignore());

        CreateMap<Specialization, SpecializationResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}