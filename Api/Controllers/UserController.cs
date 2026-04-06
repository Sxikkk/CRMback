using Application.Features.Users.Commands.ChangeUserInfo;
using Application.Features.Users.Commands.ChangeUserPassword;
using Application.Features.Users.Queries.GetAllOrganizationUsers;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{organizationId:guid}/all")]
    public async Task<IActionResult> GetUsersAsync([FromRoute] Guid organizationId, CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new GetAllOrganizationUsersQuery(organizationId), cancellationToken);
        return Ok(users);
    }

    [Authorize]
    [HttpGet("profile/{userId:guid}")]
    public async Task<IActionResult> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserByIdQuery { Id = userId }, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("profile/me")]
    public async Task<IActionResult> GetUserAsync(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserByIdQuery(), cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("profile/change")]
    public async Task<IActionResult> ChangeUserProfileAsync([FromBody] ChangeUserInfoCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("profile/change-password")]
    public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangeUserPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
