namespace Domain.Interfaces.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string username);
}