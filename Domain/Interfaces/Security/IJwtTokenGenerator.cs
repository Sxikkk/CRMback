using Domain.Enums;

namespace Domain.Interfaces.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string username, ERole role);
    string GenerateRefreshToken();
    string HashToken(string token);
    (string accessToken, string refreshToken, TimeSpan refreshExpires) GenerateTokens(Guid userId, string username, ERole role);
}
