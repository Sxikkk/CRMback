using Contracts.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Refresh;

public record RefreshCommand: IRequest<TokenDto>
{
    public string Token { get; init; }
};