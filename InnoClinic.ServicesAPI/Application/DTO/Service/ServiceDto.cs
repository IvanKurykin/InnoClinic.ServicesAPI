using Domain.Enums;

namespace Application.DTO.Service;

public class ServiceDto
{
    public Guid CategoryId { get; set; }
    public Guid SpecializationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Statuses Status { get; set; } 
}