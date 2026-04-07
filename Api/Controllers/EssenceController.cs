using Application.Features.Essence.Commands.CreateEssence;
using Application.Features.Essence.Commands.DeleteEssence;
using Application.Features.Essence.Commands.UpdateAssignedToEssence;
using Application.Features.Essence.Commands.UpdateEssenceDetails;
using Application.Features.Essence.Commands.UpdateEssenceDueDate;
using Application.Features.Essence.Commands.UpdateEssencePrice;
using Application.Features.Essence.Commands.UpdateStatusPriority;
using Application.Features.Essence.Queries.GetEssenceById;
using Application.Features.Essence.Queries.GetEssenceByUserId;
using Contracts.Tasks;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/essence")]
[Authorize]
public class EssenceController : ControllerBase
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

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new GetEssenceByUserIdQuery
        {
            UserId = userId
        }, cancellationToken);
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

    [HttpPut("{essenceId:guid}/update/price")]
    public async Task<IActionResult> UpdateEssencePriceAsync([FromRoute] Guid essenceId, [FromBody] UpdateEssencePriceRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new UpdateEssencePriceCommand(essenceId, request.Price),
            cancellationToken);
        return Ok(response);
    }

    [HttpPut("{essenceId:guid}/update/due-date")]
    public async Task<IActionResult> UpdateEssenceDueDateAsync([FromRoute] Guid essenceId, [FromBody] UpdateEssenceDueDateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new UpdateEssenceDueDateCommand(essenceId, request.DueDate), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{essenceId:guid}/update/details")]
    public async Task<IActionResult> UpdateEssenceDetailsAsync([FromRoute] Guid essenceId,
        [FromBody] UpdateEssenceDetailsRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new UpdateEssenceDetailsCommand(essenceId, request.Title, request.Description),
            cancellationToken);
        return Ok(response);
    }

    [HttpPut("{essenceId:guid}/update/priority")]
    public async Task<IActionResult> UpdateEssenceStatusAsync([FromRoute] Guid essenceId,
        [FromBody] UpdateEssenceStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new UpdateStatusPriorityCommand(essenceId, request.Priority),
            cancellationToken);
        return Ok(response);
    }

    [HttpPut("{essenceId:guid}/update/assign")]
    public async Task<IActionResult> UpdateEssenceAssignAsync([FromRoute] Guid essenceId,
        [FromBody] UpdateEssenceAssignRecord request,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new UpdateAssignedToEssenceCommand(essenceId, request.AssignedToId),
            cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveEssenceAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new DeleteEssenceCommand
        {
            EssenceId = id
        }, cancellationToken);
        return Ok(response);
    }
}