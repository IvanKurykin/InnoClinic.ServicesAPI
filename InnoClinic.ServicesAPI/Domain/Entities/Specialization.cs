using Domain.Constants;

namespace Domain.Entities;

public sealed class Specialization
{
    public Guid Id { get; set; }
    public string Status { get; set; } = Statuses.Inactive;
    public ICollection<Service> Services { get; set; } = [];
}