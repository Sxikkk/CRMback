using Contracts.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public record LoginCommand(
    Guid OrganizationId,
    string Login,
    string Password
) : IRequest<TokenDto>;