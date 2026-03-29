using Application.Features.Analytics.Queries.GetEssenceStatusStatistic;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("essence-status-statistic/{organizationId:guid}")]
    public async Task<IActionResult> GetEssenceStatusStatisticsAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetEssenceStatusStatisticQuery(organizationId), cancellationToken);
        
        return Ok(response);
    }
}