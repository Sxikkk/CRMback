using MediatR;

namespace Application.Features.EssenceStages.Commands.CompleteStage;

public sealed record CompleteStageCommand(Guid essenceId, Guid stageId) : IRequest<Guid>;
