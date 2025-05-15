using Application.DTO.Specialization;
using Application.Validators;
using FluentValidation.TestHelper;
using UnitTests.TestCases;

namespace UnitTests.Validators;
public class SpecializationRequestDtoValidatorTests
{
    private readonly SpecializationRequestDtoValidator _validator = new();

    [Fact]
    public void ShouldPassWhenValidData()
    {
        var model = new SpecializationCreateRequestDto
        {
            Name = ValidatorTestData.ValidName,
            Status = ValidatorTestData.DefaultStatus
        };

        _validator.TestValidate(model).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldNotFailWhenNameIsNullBecauseOfDefaultValue()
    {
        var model = new SpecializationCreateRequestDto
        {
            Name = default!,
            Status = ValidatorTestData.DefaultStatus
        };

        _validator.TestValidate(model).ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}