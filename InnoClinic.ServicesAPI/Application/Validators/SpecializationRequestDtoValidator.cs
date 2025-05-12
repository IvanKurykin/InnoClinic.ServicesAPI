using Application.DTO.Specialization;
using Application.Helpers;
using FluentValidation;

namespace Application.Validators;

public class SpecializationRequestDtoValidator : AbstractValidator<SpecializationCreateRequestDto>
{
    public SpecializationRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status is required and must be a valid value.");
    }
}