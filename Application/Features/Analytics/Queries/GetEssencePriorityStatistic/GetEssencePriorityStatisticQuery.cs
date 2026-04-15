using Contracts.Analytics;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssencePriorityStatistic;

public record GetEssencePriorityStatisticQuery(
    Guid OrganizationId,
    DateTime FromUtc,
    DateTime ToUtc): IRequest<EssencePriorityStatisticDto>;
