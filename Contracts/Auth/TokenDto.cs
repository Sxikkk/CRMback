namespace Contracts.Auth;

public sealed record TokenDto
{
    public string AccessToken { get; init; }
    public string? RefreshToken { get; init; }
};