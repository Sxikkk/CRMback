using Contracts.Analytics;
using Domain.Enums;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssenceTrendStatistic;

public record GetEssenceTrendStatisticQuery(
    Guid OrganizationId,
    DateTime FromUtc,
    DateTime ToUtc,
    EAnalyticsGroupBy GroupBy): IRequest<EssenceTrendStatisticDto>;
