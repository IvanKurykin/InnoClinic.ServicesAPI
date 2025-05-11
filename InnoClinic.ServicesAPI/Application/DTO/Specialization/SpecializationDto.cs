using Domain.Enums;

namespace Application.DTO.Specialization;

public class SpecializationDto
{
    public string Name { get; set; } = string.Empty;
    public Statuses Status { get; set; } 
}