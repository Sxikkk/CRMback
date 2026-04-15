using Contracts.Analytics;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssencePriorityStatistic;

public class GetEssencePriorityStatisticQueryHandler
    : IRequestHandler<GetEssencePriorityStatisticQuery, EssencePriorityStatisticDto>
{
    private readonly IEssenceRepository _essenceRepository;

    public GetEssencePriorityStatisticQueryHandler(IEssenceRepository essenceRepository)
    {
        _essenceRepository = essenceRepository;
    }

    public async Task<EssencePriorityStatisticDto> Handle(GetEssencePriorityStatisticQuery request, CancellationToken cancellationToken)
    {
        ValidatePeriod(request.FromUtc, request.ToUtc);

        var stats = await _essenceRepository.GetPriorityStatisticByOrganizationAsync(
            request.OrganizationId,
            request.FromUtc,
            request.ToUtc,
            cancellationToken);

        return new EssencePriorityStatisticDto
        {
            Normal = stats.GetValueOrDefault(EEssencePriority.Normal, 0),
            Warning = stats.GetValueOrDefault(EEssencePriority.Warning, 0),
            Danger = stats.GetValueOrDefault(EEssencePriority.Danger, 0)
        };
    }

    private static void ValidatePeriod(DateTime fromUtc, DateTime toUtc)
    {
        if (fromUtc > toUtc)
            throw new ApplicationException("FromUtc cannot be greater than ToUtc");
    }
}
