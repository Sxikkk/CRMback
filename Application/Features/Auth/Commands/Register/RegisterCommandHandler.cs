using Contracts.Auth;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Security;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler: IRequestHandler<RegisterCommand, TokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<TokenDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existUser = await _userRepository.GetUserByEmailAsync(request.Email);

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

        await _userRepository.AddUserAsync(user);
        await _userRepository.SaveAsync();

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName);
        
        return new TokenDto(token);
    }
}