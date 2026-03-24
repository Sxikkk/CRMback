using Domain.Exceptions;

namespace Domain.ValueObjects;

public readonly record struct EssencePrice
{
    public decimal Value { get; init; }

    public EssencePrice(decimal value)
    {
        if (value < 0) throw new DomainException("Price cannot be negative");
        Value = value;
    }

    public static implicit operator decimal(EssencePrice price) => price.Value;
    public static explicit operator EssencePrice(decimal value) => new(value);

    public static EssencePrice Zero => new(0m);
}