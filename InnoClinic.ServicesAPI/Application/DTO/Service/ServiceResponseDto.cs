using Application.DTO.ServiceCategory;
using Application.DTO.Specialization;

namespace Application.DTO.Service;

public sealed class ServiceResponseDto : ServiceDto
{
    public Guid Id { get; set; }
    public ServiceCategoryResponseDto? Category { get; set; }
    public SpecializationResponseDto? Specialization { get; set; }
}