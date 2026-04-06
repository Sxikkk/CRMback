using Application.Features.EssenceStages.Commands.AddStage;
using Application.Features.EssenceStages.Commands.CompleteStage;
using Application.Features.EssenceStages.Commands.PauseStage;
using Application.Features.EssenceStages.Commands.ReopenStage;
using Application.Features.EssenceStages.Commands.ReorderStages;
using Application.Features.EssenceStages.Commands.StartStage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/stages")]
[Authorize]
public class StageController : ControllerBase
{
    private readonly IMediator _mediator;

    public StageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add-stage")]
    public async Task<IActionResult> CreateStageForEssence(AddStageCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{essenceId:guid}/{stageId:guid}/start")]
    public async Task<IActionResult> StartStageAsync([FromRoute] Guid essenceId, [FromRoute] Guid stageId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new StartStageCommand(essenceId, stageId), cancellationToken);
        return Ok(response);
    }

    [HttpPatch("{essenceId:guid}/{stageId:guid}/pause")]
    public async Task<IActionResult> PauseStageAsync([FromRoute] Guid essenceId, [FromRoute] Guid stageId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new PauseStageCommand(essenceId, stageId), cancellationToken);
        return Ok(response);
    }

    [HttpPatch("{essenceId:guid}/{stageId:guid}/complete")]
    public async Task<IActionResult> CompleteStageAsync([FromRoute] Guid essenceId, [FromRoute] Guid stageId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CompleteStageCommand(essenceId, stageId), cancellationToken);
        return Ok(response);
    }

    [HttpPatch("{essenceId:guid}/{stageId:guid}/reopen")]
    public async Task<IActionResult> ReopenStageAsync([FromRoute] Guid essenceId, [FromRoute] Guid stageId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ReopenStageCommand(essenceId, stageId), cancellationToken);
        return Ok(response);
    }

    [HttpPatch("{essenceId:guid}/reorder")]
    public async Task<IActionResult> ReorderStagesAsync([FromRoute] Guid essenceId,
        [FromBody] IReadOnlyList<StageOrderChangeItem> changes,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ReorderStagesCommand(essenceId, changes), cancellationToken);
        return Ok(response);
    }
}
