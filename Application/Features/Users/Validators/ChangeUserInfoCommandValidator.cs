using Application.Features.Users.Commands.ChangeUserInfo;
using FluentValidation;

namespace Application.Features.Users.Validators;

public sealed class ChangeUserInfoCommandValidator : AbstractValidator<ChangeUserInfoCommand>
{
    public ChangeUserInfoCommandValidator()
    {
        RuleFor(x => x)
            .Must(HaveAtLeastOneValue)
            .WithMessage("At least one field must be provided");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty")
            .When(x => x.Name is not null);

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("Surname cannot be empty")
            .When(x => x.Surname is not null);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username cannot be empty")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters")
            .When(x => x.UserName is not null);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email format")
            .When(x => x.Email is not null);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone cannot be empty")
            .Must(BeValidPhone)
            .WithMessage("Invalid phone format")
            .When(x => x.Phone is not null);
    }

    private static bool HaveAtLeastOneValue(ChangeUserInfoCommand command) =>
        !string.IsNullOrWhiteSpace(command.Name) ||
        !string.IsNullOrWhiteSpace(command.Surname) ||
        !string.IsNullOrWhiteSpace(command.UserName) ||
        !string.IsNullOrWhiteSpace(command.Email) ||
        !string.IsNullOrWhiteSpace(command.Phone);

    private static bool BeValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        var cleaned = phone.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");

        return cleaned.StartsWith('+') &&
               cleaned.Length >= 10 &&
               cleaned.Length <= 15 &&
               cleaned[1..].All(char.IsDigit);
    }
}
