using Contracts.Organization;
using MediatR;

namespace Application.Features.Organization.Queries.GetAllOrganizations;

public sealed record GetAllOrganizationsQuery(): IRequest<IList<SimpleOrganizationDto>>;