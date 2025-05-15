using Domain.Enums;

namespace UnitTests.TestCases;

public static class ValidatorTestData
{
    public const string TestName = "Test Name";
    public const string ValidName = "Valid Name";
    public const string NameRequiredMessage = "Name is required.";
    public const string NameMaxLengthMessage = "Name must not exceed 100 characters.";
    public const Statuses DefaultStatus = Statuses.Active;
    public static readonly Guid TestId = Guid.NewGuid();
    public static readonly Guid EmptyId = Guid.Empty;

    public const string TimeSlotMinDurationMessage = "Time slot duration in minutes must be greater than 30.";
    public const string TimeSlotMaxDurationMessage = "Time slot duration in minutes must not exceed 1440 minutes (24 hours).";
    public const int TimeSlotDurationInMinutesSixty = 60;
    public const int TimeSlotDurationInMinutesOneThousandFourHundredAndFortyOne = 1441;

    public const string CategoryIdRequiredMessage = "CategoryId is required.";
    public const string SpecializationIdRequiredMessage = "SpecializationId is required.";
    public const string PriceMessage = "Price must be greater than 0.";
    public const string StatusEnumMessage = "Status is required and must be a valid value.";
    public const decimal ValidPrice = 100.50m;
    public const decimal InvalidPrice = -10.50m;
}