using Application.Features.Essence.Commands.CreateEssence;
using Application.Features.Essence.Commands.DeleteEssence;
using Application.Features.Essence.Commands.UpdateAssignedToEssence;
using Application.Features.Essence.Commands.UpdateEssenceDetails;
using Application.Features.Essence.Commands.UpdateStatusPriority;
using Application.Features.Essence.Queries.GetEssenceById;
using Application.Features.Essence.Queries.GetEssenceByUserId;
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
    public async Task<IActionResult> GetMyEssenceAsync(CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new GetEssenceByUserIdQuery(), cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEssenceByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new GetEssenceByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateEssenceAsync([FromBody] CreateEssenceCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [HttpPut("update/details")]
    public async Task<IActionResult> UpdateEssenceDetailsAsync([FromBody] UpdateEssenceDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
    
    [HttpPut("update/status")]
    public async Task<IActionResult> UpdateEssenceStatusAsync([FromBody] UpdateStatusPriorityCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
    
    [HttpPut("update/assign")]
    public async Task<IActionResult> UpdateEssenceAssignAsync([FromBody] UpdateAssignedToEssenceCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("update/remove")]
    public async Task<IActionResult> RemoveEssenceAsync([FromBody] DeleteEssenceCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}