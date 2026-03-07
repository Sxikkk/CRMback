using Domain.Exceptions;

namespace Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string TokenHash { get; private set; } = null!;
    public string? ReplacedByTokenHash { get; private set; }

    public string CreatedByIp { get; private set; } = null!;
    public string? RevokedByIp { get; private set; }

    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }

    public bool IsRevoked => RevokedAtUtc != null;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;

    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken()
    {
    }

    private RefreshToken(
        Guid userId,
        string tokenHash,
        DateTime expiresAtUtc,
        string createdByIp)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
        CreatedAtUtc = DateTime.UtcNow;
        CreatedByIp = createdByIp;
    }

    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        TimeSpan lifetime,
        string createdByIp)
    {
        return new RefreshToken(
            userId,
            tokenHash,
            DateTime.UtcNow.Add(lifetime),
            createdByIp
        );
    }

    public void Revoke(string ipAddress)
    {
        if (IsRevoked)
            throw new DomainException("Token already revoked");

        RevokedAtUtc = DateTime.UtcNow;
        RevokedByIp = ipAddress;
    }

    public RefreshToken Rotate(
        string newTokenHash,
        TimeSpan lifetime,
        string ipAddress)
    {
        if (!IsActive)
            throw new DomainException("Cannot rotate inactive token");

        Revoke(ipAddress);

        ReplacedByTokenHash = newTokenHash;

        return Create(
            UserId,
            newTokenHash,
            lifetime,
            ipAddress
        );
    }
}