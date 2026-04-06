using MediatR;

namespace Application.Features.EssenceStages.Commands.PauseStage;

public sealed record PauseStageCommand(Guid essenceId, Guid stageId) : IRequest<Guid>;
