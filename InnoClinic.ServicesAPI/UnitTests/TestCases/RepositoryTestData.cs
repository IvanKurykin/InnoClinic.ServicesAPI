using Domain.Entities;
using Domain.Enums;

namespace UnitTests.TestCases;

public static class RepositoryTestData
{
    public static readonly Guid TestId = Guid.NewGuid();
    public static readonly Guid NonExistentId = Guid.NewGuid();

    public static ServiceCategory CreateTestServiceCategory() => new()
    {
        Id = TestId,
        Name = "Test Category",
        TimeSlotDurationInMinutes = 30
    };

    public static Service CreateTestService() => new()
    {
        Id = TestId,
        Name = "Test Service",
        Price = 100.50m,
        Status = Statuses.Active,
        CategoryId = TestId,
        SpecializationId = TestId
    };

    public static Specialization CreateTestSpecialization() => new()
    {
        Id = TestId,
        Name = "Test Specialization",
        Status = Statuses.Active
    };
}
