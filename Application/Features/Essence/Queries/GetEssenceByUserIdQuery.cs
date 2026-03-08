using Contracts.Tasks;
using MediatR;

namespace Application.Features.Essence.Queries;

public sealed record GetEssenceByUserIdQuery: IRequest<IReadOnlyList<EssenceDto>>;