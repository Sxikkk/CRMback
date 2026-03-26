using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<RefreshToken?> GetTokenByHashAsync(string hash, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash, cancellationToken: cancellationToken);
    }

    public async Task AddTokenAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(token, cancellationToken);
    }

    public async Task<IList<RefreshToken>> GetUserTokensAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<IList<RefreshToken>> GetUserInactiveTokensAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.Where(t => t.UserId == userId && t.IsRevoked).ToListAsync(cancellationToken);
    }

    public void RemoveTokenAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        _context.RefreshTokens.Remove(token);
    }
}