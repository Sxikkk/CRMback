using Contracts.Auth;
using MediatR;

namespace Application.Features.M2m.Commands.GetServiceToken;

public sealed record GetServiceTokenCommand: IRequest<TokenDto>
{
    public string? Login { get; init; }
}
