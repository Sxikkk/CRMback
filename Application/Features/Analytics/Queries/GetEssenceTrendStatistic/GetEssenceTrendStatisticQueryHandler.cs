using Contracts.Analytics;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssenceTrendStatistic;

public class GetEssenceTrendStatisticQueryHandler
    : IRequestHandler<GetEssenceTrendStatisticQuery, EssenceTrendStatisticDto>
{
    private readonly IEssenceRepository _essenceRepository;

    public GetEssenceTrendStatisticQueryHandler(IEssenceRepository essenceRepository)
    {
        _essenceRepository = essenceRepository;
    }

    public async Task<EssenceTrendStatisticDto> Handle(GetEssenceTrendStatisticQuery request, CancellationToken cancellationToken)
    {
        ValidatePeriod(request.FromUtc, request.ToUtc);

        var points = await _essenceRepository.GetTrendStatisticByOrganizationAsync(
            request.OrganizationId,
            request.FromUtc,
            request.ToUtc,
            request.GroupBy,
            cancellationToken);

        return new EssenceTrendStatisticDto
        {
            Points = points
                .Select(x => new EssenceTrendPointDto
                {
                    PeriodStartUtc = x.PeriodStartUtc,
                    Created = x.Created,
                    Completed = x.Completed
                })
                .ToList()
        };
    }

    private static void ValidatePeriod(DateTime fromUtc, DateTime toUtc)
    {
        if (fromUtc > toUtc)
            throw new ApplicationException("FromUtc cannot be greater than ToUtc");
    }
}
