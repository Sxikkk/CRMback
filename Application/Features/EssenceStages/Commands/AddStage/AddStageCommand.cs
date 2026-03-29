using MediatR;

namespace Application.Features.EssenceStages.Commands.AddStage;

public sealed record AddStageCommand: IRequest<Guid>
{
    public Guid EssenceId { get; init; }
    public string Name { get; init; } = null!;
    public int Order { get; init; }
    public Guid? ResponsibleId { get; init; }

    public int? EstimatedDurationMinutes { get; init; }
};