using Contracts.Analytics;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssenceStatusStatistic;

public record GetEssenceStatusStatisticQuery(Guid organizationId): IRequest<EssenceStatusStatisticDto>;