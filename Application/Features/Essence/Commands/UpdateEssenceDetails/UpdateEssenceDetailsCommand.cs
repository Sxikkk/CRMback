using Domain.Enums;
using MediatR;

namespace Application.Features.Essence.Commands.UpdateEssenceDetails;

public record UpdateEssenceDetailsCommand(
    Guid id,
    string title,
    string? description
) : IRequest<Guid>;