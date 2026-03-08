using Application.Features.Auth.Commands.Login;
using FluentValidation;

namespace Application.Features.Auth.Validators;

public class LoginValidator: AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Login).NotEmpty().WithMessage("Login is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}