using Domain.Enums;

namespace Domain.Entities;

public sealed class Specialization
{
    public Guid Id { get; set; }
    public Statuses Status { get; set; } = Statuses.Inactive;
    public IEnumerable<Service> Services { get; set; } = [];
}