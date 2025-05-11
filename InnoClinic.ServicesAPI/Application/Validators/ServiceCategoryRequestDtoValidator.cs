using Application.DTO.ServiceCategory;
using FluentValidation;

namespace Application.Validators;

public class ServiceCategoryRequestDtoValidator : AbstractValidator<ServiceCategoryRequestDto>
{
    public ServiceCategoryRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.TimeSlotDuration)
            .GreaterThan(0).WithMessage("TimeSlotSize must be greater than 0.")
            .LessThanOrEqualTo(1440).WithMessage("TimeSlotSize must not exceed 1440 minutes (24 hours).");
    }
}