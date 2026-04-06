using Application.Features.EssenceStages.Commands.StartStage;
using FluentValidation;

namespace Application.Features.EssenceStages.Validators;

public sealed class StartStageCommandValidator : AbstractValidator<StartStageCommand>
{
    public StartStageCommandValidator()
    {
        RuleFor(x => x.essenceId).NotEmpty().WithMessage("EssenceId is required");
        RuleFor(x => x.stageId).NotEmpty().WithMessage("StageId is required");
    }
}
