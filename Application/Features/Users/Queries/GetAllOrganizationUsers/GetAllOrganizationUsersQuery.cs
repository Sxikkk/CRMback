using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetAllOrganizationUsers;

public record GetAllOrganizationUsersQuery(Guid organizationId): IRequest<IEnumerable<User>>;