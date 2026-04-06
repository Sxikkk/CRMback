using Application.Features.Organization.Commands.ChangeOrganizationStatus;
using Application.Features.Organization.Commands.ChangeOrganizationType;
using Application.Features.Organization.Commands.CreateOrganization;
using Application.Features.Organization.Queries.GetAllOrganizations;
using Application.Features.Organization.Queries.GetOrganizationInfo;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/organizations")]
[Authorize(Policy = "OrganizationsRead")]
public class OrganizationController : ControllerBase
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
    
    [HttpGet("{organizationId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOrganizationInfo([FromRoute] Guid organizationId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetOrganizationInfoQuery(organizationId), cancellationToken);

        return Ok(response);
    }

    [HttpPost("create")]
    [Authorize(Policy = "OrganizationsWrite")]
    public async Task<IActionResult> CreateOrganizationAsync(CreateOrganizationCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    [HttpPut("{organizationId:guid}/change-status")]
    [Authorize(Policy = "OrganizationsWrite")]
    public async Task<IActionResult> ChangeOrganizationStatusAsync([FromRoute] Guid organizationId,
        [FromBody] EOrganizationStatus status,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ChangeOrganizationStatusCommand(organizationId, status),
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("{organizationId:guid}/change-type")]
    [Authorize(Policy = "OrganizationsWrite")]
    public async Task<IActionResult> ChangeOrganizationTypeAsync([FromRoute] Guid organizationId,
        [FromBody] EOrganizationType type,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ChangeOrganizationTypeCommand(organizationId, type), cancellationToken);

        return Ok(response);
    }
}