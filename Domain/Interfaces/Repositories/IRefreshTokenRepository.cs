using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetTokenByHashAsync(string hash, CancellationToken cancellationToken);
    Task AddTokenAsync(RefreshToken token, CancellationToken cancellationToken);
    Task<IList<RefreshToken>> GetUserTokensAsync(Guid userId, CancellationToken cancellationToken);
    Task<IList<RefreshToken>> GetUserInactiveTokensAsync(Guid userId, CancellationToken cancellationToken);
    void RemoveTokenAsync(RefreshToken token, CancellationToken cancellationToken);
}