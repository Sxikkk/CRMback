using Contracts.User;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery: IRequest<List<UserDto>>;