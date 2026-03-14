using MediatR;

namespace Application.Features.Essence.Commands.UpdateAssignedToEssence;

public sealed record UpdateAssignedToEssenceCommand: IRequest<Guid>
{
    public Guid EssenceId { get; init; }
    public Guid? AssignedToId { get; init; }
}