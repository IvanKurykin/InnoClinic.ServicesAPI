namespace Domain.Entities;

public sealed class ServiceCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TimeSlotSize { get; set; }
    public ICollection<Service> Services { get; set; } = [];
}