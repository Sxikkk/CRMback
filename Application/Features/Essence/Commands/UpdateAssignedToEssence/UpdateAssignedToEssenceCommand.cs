using MediatR;

namespace Application.Features.Essence.Commands.UpdateAssignedToEssence;

public sealed record UpdateAssignedToEssenceCommand(Guid essenceId, Guid? assignedToId) : IRequest<Guid>;