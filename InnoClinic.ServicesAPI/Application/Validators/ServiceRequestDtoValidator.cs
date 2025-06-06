﻿using Application.DTO.Service;
using FluentValidation;

namespace Application.Validators;

public class ServiceRequestDtoValidator : AbstractValidator<ServiceCreateRequestDto>
{
    public ServiceRequestDtoValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.SpecializationId)
            .NotEmpty().WithMessage("SpecializationId is required.");

        RuleFor(x => x.Name ?? "Initial inspection")
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status is required and must be a valid value.");
    }
}