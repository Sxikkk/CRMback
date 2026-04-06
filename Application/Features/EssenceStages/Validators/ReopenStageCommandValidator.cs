using Application.Features.EssenceStages.Commands.ReopenStage;
using FluentValidation;

namespace Application.Features.EssenceStages.Validators;

public sealed class ReopenStageCommandValidator : AbstractValidator<ReopenStageCommand>
{
    public ReopenStageCommandValidator()
    {
        RuleFor(x => x.essenceId).NotEmpty().WithMessage("EssenceId is required");
        RuleFor(x => x.stageId).NotEmpty().WithMessage("StageId is required");
    }
}
