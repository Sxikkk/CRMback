using Application.Features.EssenceStages.Commands.CompleteStage;
using FluentValidation;

namespace Application.Features.EssenceStages.Validators;

public sealed class CompleteStageCommandValidator : AbstractValidator<CompleteStageCommand>
{
    public CompleteStageCommandValidator()
    {
        RuleFor(x => x.essenceId).NotEmpty().WithMessage("EssenceId is required");
        RuleFor(x => x.stageId).NotEmpty().WithMessage("StageId is required");
    }
}
