using Domain.Exceptions;

namespace Domain.ValueObjects;

public record Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string email)
    {
        if (!IsValidFormat(email, out var trimmed))
            throw new ArgumentException("Invalid email format.");

        return new Email(trimmed);
    }

    private static bool IsValidFormat(string input, out string trimmed)
    {
        trimmed = null;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        trimmed = input.Trim();

        try
        {
            var addr = new System.Net.Mail.MailAddress(trimmed);
            return addr.Address == trimmed;
        }
        catch
        {
            return false;
        }
    }
    
    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string value) => Create(value);
}