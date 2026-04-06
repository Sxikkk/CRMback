using Application.Features.EssenceStages.Commands.PauseStage;
using FluentValidation;

namespace Application.Features.EssenceStages.Validators;

public sealed class PauseStageCommandValidator : AbstractValidator<PauseStageCommand>
{
    public PauseStageCommandValidator()
    {
        RuleFor(x => x.essenceId).NotEmpty().WithMessage("EssenceId is required");
        RuleFor(x => x.stageId).NotEmpty().WithMessage("StageId is required");
    }
}
