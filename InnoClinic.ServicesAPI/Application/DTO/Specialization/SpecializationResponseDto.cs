using Application.DTO.Service;

namespace Application.DTO.Specialization;

public class SpecializationResponseDto : SpecializationDto
{
    public Guid Id { get; set; }
    public IEnumerable<ServiceDto> Services = [];
}