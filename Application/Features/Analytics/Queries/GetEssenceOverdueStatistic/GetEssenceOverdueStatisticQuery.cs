using Contracts.Analytics;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssenceOverdueStatistic;

public record GetEssenceOverdueStatisticQuery(
    Guid OrganizationId,
    DateTime FromUtc,
    DateTime ToUtc): IRequest<EssenceOverdueStatisticDto>;
