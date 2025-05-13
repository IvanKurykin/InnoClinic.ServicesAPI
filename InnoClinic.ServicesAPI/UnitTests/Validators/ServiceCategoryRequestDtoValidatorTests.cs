using Application.DTO.ServiceCategory;
using Application.Validators;
using FluentValidation.TestHelper;
using UnitTests.TestCases;

namespace UnitTests.Validators;

public class ServiceCategoryRequestDtoValidatorTests
{
    private readonly ServiceCategoryRequestDtoValidator _validator = new();

    [Fact]
    public void ShouldPassWhenValidData()
    {
        var model = new ServiceCategoryCreateRequestDto
        {
            Name = ValidatorTestData.ValidName,
            TimeSlotDurationInMinutes = ValidatorTestData.TimeSlotDurationInMinutesSixty
        };

        _validator.TestValidate(model).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldNotFailWhenNameIsNullBecauseOfDefaultValue()
    {
        var model = new ServiceCategoryCreateRequestDto
        {
            Name = default!,
            TimeSlotDurationInMinutes = ValidatorTestData.TimeSlotDurationInMinutesSixty
        };

        _validator.TestValidate(model).ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldNotFailWhenNameIsEmptyBecauseOfDefaultValue()
    {
        var model = new ServiceCategoryCreateRequestDto
        {
            Name = "",
            TimeSlotDurationInMinutes = ValidatorTestData.TimeSlotDurationInMinutesSixty
        };

        _validator.TestValidate(model).ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldNotFailWhenNameIsWhitespaceBecauseOfDefaultValue()
    {
        var model = new ServiceCategoryCreateRequestDto
        {
            Name = "   ",
            TimeSlotDurationInMinutes = ValidatorTestData.TimeSlotDurationInMinutesSixty
        };

        _validator.TestValidate(model).ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    [InlineData(-10)]
    public void ShouldFailWhenTimeSlotDurationTooSmall(int duration)
    {
        var model = new ServiceCategoryCreateRequestDto
        {
            Name = ValidatorTestData.TestName,
            TimeSlotDurationInMinutes = duration
        };

        _validator.TestValidate(model).ShouldHaveValidationErrorFor(x => x.TimeSlotDurationInMinutes).WithErrorMessage(ValidatorTestData.TimeSlotMinDurationMessage);
    }

    [Fact]
    public void ShouldFailWhenTimeSlotDurationTooLarge()
    {
        var model = new ServiceCategoryCreateRequestDto
        {
            Name = ValidatorTestData.TestName,
            TimeSlotDurationInMinutes = ValidatorTestData.TimeSlotDurationInMinutesOneThousandFourHundredAndFortyOne
        };

        _validator.TestValidate(model).ShouldHaveValidationErrorFor(x => x.TimeSlotDurationInMinutes).WithErrorMessage(ValidatorTestData.TimeSlotMaxDurationMessage);
    }
}