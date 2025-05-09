using Domain.Constants;

namespace Domain.Entities;

public sealed class Service
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public ServiceCategory? Category { get; set; }
    public Guid SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } 
    public Statuses Status { get; set; } = Statuses.Inactive;
}