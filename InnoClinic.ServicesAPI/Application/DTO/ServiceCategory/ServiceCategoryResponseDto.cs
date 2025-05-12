using Application.DTO.Service;

namespace Application.DTO.ServiceCategory;

public class ServiceCategoryResponseDto : ServiceCategoryDto
{ 
    public Guid Id { get; set; }
    public IReadOnlyCollection<ServiceResponseDto> Services { get; set; } = [];
}