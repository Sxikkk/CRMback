using Domain.Enums;
using MediatR;

namespace Application.Features.Organization.Commands.CreateOrganization;

public sealed record CreateOrganizationCommand: IRequest<Guid>
{
    public string OrganizationName { get; init; } = null!;
    public EOrganizationType  OrganizationType { get; init; }
    public string? Inn { get; init; }
    public string? Ogrn { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Website { get; init; }
};