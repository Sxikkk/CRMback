using MediatR;

namespace Application.Features.EssenceStages.Commands.ReopenStage;

public sealed record ReopenStageCommand(Guid essenceId, Guid stageId) : IRequest<Guid>;
