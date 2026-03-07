namespace Domain.Interfaces.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string username);
    string GenerateRefreshToken();
    string HashToken(string token);
    (string accessToken, string refreshToken, TimeSpan refreshExpires) GenerateTokens(Guid userId, string username);
}