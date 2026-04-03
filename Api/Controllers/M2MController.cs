using Application.Features.M2m.Commands.GetServiceToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/m2m")]
public class M2MController: ControllerBase
{
    private readonly IMediator _mediator;

    public M2MController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceToken(GetServiceTokenCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }
}