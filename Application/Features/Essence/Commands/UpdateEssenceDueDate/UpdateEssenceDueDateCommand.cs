using MediatR;

namespace Application.Features.Essence.Commands.UpdateEssenceDueDate;

public sealed record UpdateEssenceDueDateCommand(Guid essenceId, DateTime? dueDate): IRequest<Guid>;