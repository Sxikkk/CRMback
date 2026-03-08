using Application.Common.Interfaces;
using Contracts.Auth;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Security;
using MediatR;

namespace Application.Features.Auth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, TokenDto>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRequestContext _requestContext;

    public RefreshCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenRepository refreshTokenRepository,
        IRequestContext requestContext, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _requestContext = requestContext;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var hashRefreshToken = _jwtTokenGenerator.HashToken(request.Token);

        var activeToken = await _refreshTokenRepository.GetTokenByHashAsync(hashRefreshToken, cancellationToken);

        if (activeToken == null)
            throw new UnauthorizedAccessException("Token not found");

        if (activeToken.IsRevoked || activeToken.RevokedAtUtc != null)
            throw new UnauthorizedAccessException("Token revoked");

        var ipAddress = _requestContext.IpAddress;

        if (activeToken.ExpiresAtUtc <= DateTime.UtcNow)
        {
            activeToken.Revoke(ipAddress);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            throw new UnauthorizedAccessException("Token revoked");
        }

        if (!activeToken.IsActive)
            throw new UnauthorizedAccessException("Token revoked");

        var user = await _userRepository.GetUserByIdAsync(activeToken.UserId, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
            
        }
        
        var tokens = _jwtTokenGenerator.GenerateTokens(user.Id, user.UserName);
        
        var newHashedToken = _jwtTokenGenerator.HashToken(tokens.refreshToken);
        
        var newToken = activeToken.Rotate(newHashedToken, tokens.refreshExpires, ipAddress);
        
        await _refreshTokenRepository.AddTokenAsync(newToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new TokenDto
        {
            AccessToken = tokens.accessToken,
            RefreshToken = tokens.refreshToken,
        };
    }
}
