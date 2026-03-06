using Contracts.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Login,
    string Password
) : IRequest<TokenDto>;