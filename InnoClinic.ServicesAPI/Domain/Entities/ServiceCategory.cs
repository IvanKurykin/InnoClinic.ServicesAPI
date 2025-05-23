﻿namespace Domain.Entities;

public sealed class ServiceCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TimeSlotDurationInMinutes { get; set; }
    public IEnumerable<Service> Services { get; set; } = [];
}