using Application.Common.Interfaces;
using Contracts.Auth;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Security;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, TokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRequestContext _requestContext;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository, IRequestContext requestContext, IUnitOfWork unitOfWork, IOrganizationRepository organizationRepository)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _requestContext = requestContext;
        _unitOfWork = unitOfWork;
        _organizationRepository = organizationRepository;
    }

    public async Task<TokenDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isOrgExist = await _organizationRepository.HasOrganizationAsync(request.OrganizationId, cancellationToken);

        if (isOrgExist is false)
            throw new ApplicationException("Organization not found");
        
        var existUser = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

        if (existUser is not null)
            throw new ApplicationException("User already exists");

        var email = Email.Create(request.Email);

        var phone = Phone.Create(request.Phone);

        var passwordHash =
            BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = User.Create(
            request.Name,
            request.Surname,
            email,
            phone,
            request.Username,
            passwordHash
        );

        var tokens = _jwtTokenGenerator.GenerateTokens(user.Id, user.UserName, user.Role);

        var hashRefreshToken = _jwtTokenGenerator.HashToken(tokens.refreshToken);

        var ip = _requestContext.IpAddress;
        
        var refreshToken = RefreshToken.Create(
            user.Id,
            hashRefreshToken,
            tokens.refreshExpires,
            ip
        );

        await _userRepository.AddUserAsync(user, cancellationToken);
        await _refreshTokenRepository.AddTokenAsync(refreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TokenDto
        {
            AccessToken = tokens.accessToken,
            RefreshToken = tokens.refreshToken,
        };
    }
}
