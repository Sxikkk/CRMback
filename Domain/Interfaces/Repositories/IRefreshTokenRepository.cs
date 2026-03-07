using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetTokenByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<RefreshToken?> GetTokenByHashAsync(string hash, CancellationToken cancellationToken);
    Task<ICollection<RefreshToken>> GetTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<ICollection<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task AddTokenAsync(RefreshToken token, CancellationToken cancellationToken);
    void UpdateToken(RefreshToken token);
    void RemoveToken(RefreshToken token);
    Task RemoveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}