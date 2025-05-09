using Application.DTO.Service;

namespace Application.DTO.ServiceCategory;

public class ServiceCategoryResponseDto : ServiceCategoryDto
{ 
    public Guid Id { get; set; }
    public IEnumerable<ServiceDto> Services = [];
}