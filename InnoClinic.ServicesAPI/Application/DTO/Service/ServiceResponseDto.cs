using Application.DTO.ServiceCategory;
using Application.DTO.Specialization;

namespace Application.DTO.Service;

public sealed class ServiceResponseDto : ServiceDto
{
    public Guid Id { get; set; }
    public ServiceCategoryDto? Category { get; set; }
    public SpecializationDto? Specialization { get; set; }
}