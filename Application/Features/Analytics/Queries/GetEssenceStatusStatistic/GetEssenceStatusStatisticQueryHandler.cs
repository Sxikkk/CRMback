using Contracts.Analytics;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssenceStatusStatistic;

public class GetEssenceStatusStatisticQueryHandler: IRequestHandler<GetEssenceStatusStatisticQuery, EssenceStatusStatisticDto>
{
    private readonly IEssenceRepository _essenceRepository;

    public GetEssenceStatusStatisticQueryHandler(IEssenceRepository essenceRepository)
    {
        _essenceRepository = essenceRepository;
    }

    public async Task<EssenceStatusStatisticDto> Handle(GetEssenceStatusStatisticQuery request, CancellationToken cancellationToken)
    {
        ValidatePeriod(request.FromUtc, request.ToUtc);

        var stats = await _essenceRepository.GetStatusStatisticByOrganizationAsync(
            request.OrganizationId,
            request.FromUtc,
            request.ToUtc,
            cancellationToken);

        return new EssenceStatusStatisticDto
        {
            Waiting = stats.GetValueOrDefault(EEssenceStatus.Waiting, 0),
            Paused = stats.GetValueOrDefault(EEssenceStatus.Paused, 0),
            InProgress = stats.GetValueOrDefault(EEssenceStatus.InProgress, 0),
            Completed = stats.GetValueOrDefault(EEssenceStatus.Completed, 0)
        };
    }

    private static void ValidatePeriod(DateTime fromUtc, DateTime toUtc)
    {
        if (fromUtc > toUtc)
            throw new ApplicationException("FromUtc cannot be greater than ToUtc");
    }
}