using MediatR;

namespace Application.Features.Essence.Commands.DeleteEssence;

public sealed record DeleteEssenceCommand : IRequest<bool>
{
    public Guid EssenceId { get; init; }
};