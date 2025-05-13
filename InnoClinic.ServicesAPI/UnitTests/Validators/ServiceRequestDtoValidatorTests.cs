using Application.DTO.Service;
using Application.Validators;
using FluentValidation.TestHelper;
using UnitTests.TestCases;

namespace UnitTests.Validators;

public class ServiceRequestDtoValidatorTests
{
    private readonly ServiceRequestDtoValidator _validator = new();

    [Fact]
    public void ShouldPassWhenValidData()
    {
        var model = new ServiceCreateRequestDto
        {
            CategoryId = ValidatorTestData.TestId,
            SpecializationId = ValidatorTestData.TestId,
            Name = ValidatorTestData.ValidName,
            Price = ValidatorTestData.ValidPrice,
            Status = ValidatorTestData.DefaultStatus
        };

        _validator.TestValidate(model).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldFailWhenCategoryIdIsEmpty()
    {
        var model = new ServiceCreateRequestDto
        {
            CategoryId = ValidatorTestData.EmptyId,
            SpecializationId = ValidatorTestData.TestId,
            Name = ValidatorTestData.ValidName,
            Price = ValidatorTestData.ValidPrice,
            Status = ValidatorTestData.DefaultStatus
        };

        _validator.TestValidate(model).ShouldHaveValidationErrorFor(x => x.CategoryId).WithErrorMessage(ValidatorTestData.CategoryIdRequiredMessage);
    }

    [Fact]
    public void ShouldFailWhenSpecializationIdIsEmpty()
    {
        var model = new ServiceCreateRequestDto
        {
            CategoryId = ValidatorTestData.TestId,
            SpecializationId = ValidatorTestData.EmptyId,
            Name = ValidatorTestData.ValidName,
            Price = ValidatorTestData.ValidPrice,
            Status = ValidatorTestData.DefaultStatus
        };

        _validator.TestValidate(model)
            .ShouldHaveValidationErrorFor(x => x.SpecializationId)
            .WithErrorMessage(ValidatorTestData.SpecializationIdRequiredMessage);
    }

    [Fact]
    public void ShouldNotFailWhenNameIsNullBecauseOfDefaultValue()
    {
        var model = new ServiceCreateRequestDto
        {
            CategoryId = ValidatorTestData.TestId,
            SpecializationId = ValidatorTestData.TestId,
            Name = default!,
            Price = ValidatorTestData.ValidPrice,
            Status = ValidatorTestData.DefaultStatus
        };

        _validator.TestValidate(model).ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldFailWhenPriceIsInvalid()
    {
        var model = new ServiceCreateRequestDto
        {
            CategoryId = ValidatorTestData.TestId,
            SpecializationId = ValidatorTestData.TestId,
            Name = ValidatorTestData.ValidName,
            Price = ValidatorTestData.InvalidPrice,
            Status = ValidatorTestData.DefaultStatus
        };

        _validator.TestValidate(model).ShouldHaveValidationErrorFor(x => x.Price).WithErrorMessage(ValidatorTestData.PriceMessage);
    }
}