using Contracts.Organization;
using MediatR;

namespace Application.Features.Organization.Queries.GetOrganizationInfo;

public record GetOrganizationInfoQuery(Guid organizationId): IRequest<FullOrganizationDto>;