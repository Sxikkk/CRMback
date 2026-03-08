using Application.Features.Essence.Commands.CreateEssence;
using Application.Features.Essence.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/essence")]
[Authorize]
public class EssenceController: ControllerBase
{
    private readonly IMediator _mediator;

    public EssenceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyEssence(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetEssenceByUserIdQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateEssence([FromBody] CreateEssenceCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}