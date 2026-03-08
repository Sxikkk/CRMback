using Application.Features.Essence.Commands.CreateEssence;
using FluentValidation;

namespace Application.Features.Essence.Validators;

public class EssenceCreateValidator: AbstractValidator<CreateEssenceCommand>
{
    public EssenceCreateValidator()
    {
        RuleFor(c => c.Title).NotNull().WithMessage("Title is required").MaximumLength(100).WithMessage("Title must not exceed 100 characters");
    }
}