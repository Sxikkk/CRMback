using Application.Features.Users.Commands.ChangeUserPassword;
using FluentValidation;

namespace Application.Features.Users.Validators;

public sealed class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required")
            .MinimumLength(6)
            .WithMessage("New password must be at least 6 characters")
            .Must(ContainNumber)
            .WithMessage("New password must contain at least one number");

        RuleFor(x => x)
            .Must(x => x.CurrentPassword != x.NewPassword)
            .WithMessage("New password must be different from current password");
    }

    private static bool ContainNumber(string password) => password.Any(char.IsDigit);
}
