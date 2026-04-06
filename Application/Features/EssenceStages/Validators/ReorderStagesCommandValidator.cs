using Application.Features.EssenceStages.Commands.ReorderStages;
using FluentValidation;

namespace Application.Features.EssenceStages.Validators;

public sealed class ReorderStagesCommandValidator : AbstractValidator<ReorderStagesCommand>
{
    public ReorderStagesCommandValidator()
    {
        RuleFor(x => x.essenceId).NotEmpty().WithMessage("EssenceId is required");

        RuleFor(x => x.changes)
            .NotNull().WithMessage("Changes are required")
            .NotEmpty().WithMessage("At least one stage change is required");

        RuleForEach(x => x.changes).ChildRules(change =>
        {
            change.RuleFor(x => x.stageId).NotEmpty().WithMessage("StageId is required");
            change.RuleFor(x => x.newOrder).GreaterThanOrEqualTo(0).WithMessage("NewOrder cannot be negative");
        });
    }
}
