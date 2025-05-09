using Domain.Enums;

namespace Application.DTO.Specialization;

public class SpecializationDto
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = Statuses.Inactive.ToString();
}