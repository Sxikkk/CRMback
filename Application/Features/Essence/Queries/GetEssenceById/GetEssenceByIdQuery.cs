using Contracts.Tasks;
using MediatR;

namespace Application.Features.Essence.Queries.GetEssenceById;

public sealed record GetEssenceByIdQuery(Guid EssenceId): IRequest<EssenceDto>;