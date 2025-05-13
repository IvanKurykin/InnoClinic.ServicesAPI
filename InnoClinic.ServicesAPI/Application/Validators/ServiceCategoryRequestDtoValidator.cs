using Application.DTO.ServiceCategory;
using FluentValidation;

namespace Application.Validators;

public class ServiceCategoryRequestDtoValidator : AbstractValidator<ServiceCategoryCreateRequestDto>
{
    public ServiceCategoryRequestDtoValidator()
    {
        RuleFor(x => x.Name ?? "Basic counselling")
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.TimeSlotDurationInMinutes)
            .GreaterThan(30).WithMessage("Time slot duration in minutes must be greater than 30.")
            .LessThanOrEqualTo(1440).WithMessage("Time slot duration in minutes must not exceed 1440 minutes (24 hours).");
    }
}