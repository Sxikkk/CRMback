using Contracts.Tasks;
using MediatR;

namespace Application.Features.Essence.Queries.GetEssenceByUserId;

public sealed record GetEssenceByUserIdQuery: IRequest<IReadOnlyList<EssenceDto>>;