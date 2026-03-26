using Application.Features.Organization.Queries.GetAllOrganizations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/organizations")]
[Authorize]
public class OrganizationController: ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllShortOrganizations(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllOrganizationsQuery(), cancellationToken);
        
        return Ok(response);
    }
}