using Contracts.Tasks;
using MediatR;

namespace Application.Features.Essence.Commands.CreateEssence;

public sealed record CreateEssenceCommand: IRequest<EssenceDto>
{
    public string Title { get; init; } = null!;
}