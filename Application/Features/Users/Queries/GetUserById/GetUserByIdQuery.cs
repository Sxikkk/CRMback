using Contracts.User;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery: IRequest<UserDto>;