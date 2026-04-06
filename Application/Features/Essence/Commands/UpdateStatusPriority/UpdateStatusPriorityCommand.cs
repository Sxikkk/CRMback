using Domain.Enums;
using MediatR;

namespace Application.Features.Essence.Commands.UpdateStatusPriority;

public sealed record UpdateStatusPriorityCommand(
    Guid essenceId,
    EEssencePriority? priority
) : IRequest<Guid>;