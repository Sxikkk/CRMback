using Domain.Enums;
using MediatR;

namespace Application.Features.Organization.Commands.ChangeOrganizationStatus;

public sealed record ChangeOrganizationStatusCommand(Guid organizationId, EOrganizationStatus status): IRequest<Guid>;