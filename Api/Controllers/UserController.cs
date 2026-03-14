using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController: ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(users);
    }

    [Authorize]
    [HttpGet("profile/{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserByIdQuery{ Id = userId}, cancellationToken);
        return Ok(response);
    }
    
    [Authorize]
    [HttpGet("profile/me")]
    public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserByIdQuery(), cancellationToken);
        return Ok(response);
    }
}