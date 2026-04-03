using System.Net.Http.Json;
using Application.Common.Interfaces;
using Contracts.Auth;
using Domain.Interfaces.Security;
using MediatR;

namespace Application.Features.M2m.Commands.GetServiceToken;

public class GetServiceTokenCommandHandler: IRequestHandler<GetServiceTokenCommand, TokenDto>
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IRequestContext _context;
    private readonly string _adminApi = "http://localhost:4100/api/check-key";

    private readonly HttpClient _httpClient = new();

    private sealed record CheckKeyResponse
    {
        public string UserId { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }

    public GetServiceTokenCommandHandler(IJwtTokenGenerator tokenGenerator, IRequestContext context)
    {
        _tokenGenerator = tokenGenerator;
        _context = context;
    }

    public async Task<TokenDto> Handle(GetServiceTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_context.XAdminKey))
            throw new UnauthorizedAccessException("X-Admin-Key is missing");

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _adminApi);
        httpRequest.Headers.Add("x-admin-key", _context.XAdminKey);

        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Invalid service key");

        var checkKey = await response.Content.ReadFromJsonAsync<CheckKeyResponse>(cancellationToken: cancellationToken);
        if (checkKey is null || string.IsNullOrWhiteSpace(checkKey.Username))
            throw new UnauthorizedAccessException("Service key owner was not resolved");

        if (!string.IsNullOrWhiteSpace(request.Login) &&
            !string.Equals(request.Login, checkKey.Username, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Login does not match service key owner");

        var tokenPair = _tokenGenerator.GenerateServiceTokens(checkKey.Username);

        return new TokenDto
        {
            AccessToken = tokenPair.accessToken,
            RefreshToken = tokenPair.refreshToken,
        };
    }
}
