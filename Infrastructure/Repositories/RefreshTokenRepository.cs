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

    public async Task<RefreshToken?> GetTokenByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<RefreshToken?> GetTokenByHashAsync(string hash, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash, cancellationToken: cancellationToken);
    }

    public async Task<ICollection<RefreshToken>> GetTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<ICollection<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAtUtc == null && t.ExpiresAtUtc > DateTime.UtcNow)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task AddTokenAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(token, cancellationToken);
    }

    public void UpdateToken(RefreshToken token)
    {
        _context.RefreshTokens.Update(token);
    }

    public void RemoveToken(RefreshToken token)
    {
        _context.RefreshTokens.Remove(token);
    }

    public async Task RemoveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var tokens = await _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync(cancellationToken: cancellationToken);
        _context.RefreshTokens.RemoveRange(tokens);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}