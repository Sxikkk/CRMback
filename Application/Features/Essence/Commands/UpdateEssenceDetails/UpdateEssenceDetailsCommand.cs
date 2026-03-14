using Domain.Enums;
using MediatR;

namespace Application.Features.Essence.Commands.UpdateEssenceDetails;

public record UpdateEssenceDetailsCommand: IRequest<Guid>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
}