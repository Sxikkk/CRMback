using Contracts.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand : IRequest<TokenDto>
{
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Password { get; init; }
}