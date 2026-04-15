using Contracts.Analytics;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssenceOverdueStatistic;

public class GetEssenceOverdueStatisticQueryHandler
    : IRequestHandler<GetEssenceOverdueStatisticQuery, EssenceOverdueStatisticDto>
{
    private readonly IEssenceRepository _essenceRepository;

    public GetEssenceOverdueStatisticQueryHandler(IEssenceRepository essenceRepository)
    {
        _essenceRepository = essenceRepository;
    }

    public async Task<EssenceOverdueStatisticDto> Handle(GetEssenceOverdueStatisticQuery request, CancellationToken cancellationToken)
    {
        ValidatePeriod(request.FromUtc, request.ToUtc);

        var result = await _essenceRepository.GetOverdueStatisticByOrganizationAsync(
            request.OrganizationId,
            request.FromUtc,
            request.ToUtc,
            cancellationToken);

        var overduePercent = result.Total == 0
            ? 0m
            : Math.Round((decimal)result.Overdue / result.Total * 100m, 2);

        return new EssenceOverdueStatisticDto
        {
            Total = result.Total,
            Overdue = result.Overdue,
            OverduePercent = overduePercent,
            OverdueNormal = result.OverdueNormal,
            OverdueWarning = result.OverdueWarning,
            OverdueDanger = result.OverdueDanger
        };
    }

    private static void ValidatePeriod(DateTime fromUtc, DateTime toUtc)
    {
        if (fromUtc > toUtc)
            throw new ApplicationException("FromUtc cannot be greater than ToUtc");
    }
}
