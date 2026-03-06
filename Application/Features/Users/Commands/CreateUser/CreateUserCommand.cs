using Contracts.User;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Name,
    string Surname,
    string Email,
    string Phone,
    string UserName,
    string Password
) : IRequest<UserDto>;