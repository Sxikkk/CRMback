using Domain.Enums;
using MediatR;

namespace Application.Features.Essence.Commands.UpdateStatusPriority;

public sealed record UpdateStatusPriorityCommand: IRequest<Guid>
{
    public Guid EssenceId { get; init; }
    public EEssenceStatus? Status { get; init; }
    public EEssencePriority? Priority { get; init; }
}