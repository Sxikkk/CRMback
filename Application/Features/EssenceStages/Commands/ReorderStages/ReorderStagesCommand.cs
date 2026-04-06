using MediatR;

namespace Application.Features.EssenceStages.Commands.ReorderStages;

public sealed record ReorderStagesCommand(Guid essenceId, IReadOnlyList<StageOrderChangeItem> changes) : IRequest<Guid>;

public sealed record StageOrderChangeItem(Guid stageId, int newOrder);
