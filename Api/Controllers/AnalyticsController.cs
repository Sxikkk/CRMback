using Application.Features.Analytics.Queries.GetEssenceStatusStatistic;
using Application.Features.Analytics.Queries.GetEssencePriorityStatistic;
using Application.Features.Analytics.Queries.GetEssenceTrendStatistic;
using Application.Features.Analytics.Queries.GetEssenceOverdueStatistic;
using Domain.Enums;
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

    [HttpGet("status")]
    public async Task<IActionResult> GetEssenceStatusStatisticsAsync(
        [FromQuery] Guid organizationId,
        [FromQuery] DateTime fromUtc,
        [FromQuery] DateTime toUtc,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetEssenceStatusStatisticQuery(organizationId, fromUtc, toUtc), cancellationToken);
        
        return Ok(response);
    }

    [HttpGet("priority")]
    public async Task<IActionResult> GetEssencePriorityStatisticsAsync(
        [FromQuery] Guid organizationId,
        [FromQuery] DateTime fromUtc,
        [FromQuery] DateTime toUtc,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetEssencePriorityStatisticQuery(organizationId, fromUtc, toUtc), cancellationToken);
        
        return Ok(response);
    }

    [HttpGet("trend")]
    public async Task<IActionResult> GetEssenceTrendStatisticsAsync(
        [FromQuery] Guid organizationId,
        [FromQuery] DateTime fromUtc,
        [FromQuery] DateTime toUtc,
        [FromQuery] EAnalyticsGroupBy groupBy = EAnalyticsGroupBy.Day,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new GetEssenceTrendStatisticQuery(organizationId, fromUtc, toUtc, groupBy), cancellationToken);
        
        return Ok(response);
    }

    [HttpGet("overdue")]
    public async Task<IActionResult> GetEssenceOverdueStatisticsAsync(
        [FromQuery] Guid organizationId,
        [FromQuery] DateTime fromUtc,
        [FromQuery] DateTime toUtc,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetEssenceOverdueStatisticQuery(organizationId, fromUtc, toUtc), cancellationToken);
        
        return Ok(response);
    }
}