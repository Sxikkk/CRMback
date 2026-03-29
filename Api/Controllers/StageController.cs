using Application.Features.EssenceStages.Commands;
using Application.Features.EssenceStages.Commands.AddStage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/stages")]
[Authorize]
public class StageController: ControllerBase
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
}