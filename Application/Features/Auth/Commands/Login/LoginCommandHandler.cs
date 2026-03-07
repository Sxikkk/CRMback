using Application.Common.Interfaces;
using Contracts.Auth;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Security;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRequestContext _requestContext;

    public LoginCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator,
        IRequestContext requestContext, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _requestContext = requestContext;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Login, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new ApplicationException("Invalid credentials");

        var tokens = _jwtTokenGenerator.GenerateTokens(user.Id, user.UserName);

        var hashRefreshToken = _jwtTokenGenerator.HashToken(tokens.refreshToken);

        var ip = _requestContext.IpAddress;

        var refreshToken = RefreshToken.Create(
            user.Id,
            hashRefreshToken,
            tokens.refreshExpires,
            ip
        );

        await _refreshTokenRepository.AddTokenAsync(refreshToken, cancellationToken);
        await _userRepository.SaveAsync(cancellationToken);

        return new TokenDto
        {
            AccessToken = tokens.accessToken,
            RefreshToken = tokens.refreshToken,
        };
    }
}