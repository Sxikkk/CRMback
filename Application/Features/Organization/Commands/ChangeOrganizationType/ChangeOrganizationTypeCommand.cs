using Domain.Enums;
using MediatR;

namespace Application.Features.Organization.Commands.ChangeOrganizationType;

public record ChangeOrganizationTypeCommand(Guid organizationId, EOrganizationType Type): IRequest<Guid>;