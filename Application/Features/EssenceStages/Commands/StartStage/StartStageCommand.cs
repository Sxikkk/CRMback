using MediatR;

namespace Application.Features.EssenceStages.Commands.StartStage;

public sealed record StartStageCommand(Guid essenceId, Guid stageId) : IRequest<Guid>;
