using Domain.Exceptions;

namespace Domain.ValueObjects;

public record Phone
{
    public string Value { get; private init; }

    private Phone(string value) => Value = value;

    public static Phone Create(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone is required.");

        var cleaned = phone.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");

        if (!IsValidFormat(cleaned))
            throw new DomainException("Invalid phone format.");

        return new Phone(cleaned);
    }

    private static bool IsValidFormat(string phone)
    {
        return phone.StartsWith($"+") &&
               phone.Length >= 10 &&
               phone.Length <= 15 &&
               phone[1..].All(char.IsDigit);
    }

    public static implicit operator string(Phone phone) => phone.Value;
    public static explicit operator Phone(string value) => Create(value);
}