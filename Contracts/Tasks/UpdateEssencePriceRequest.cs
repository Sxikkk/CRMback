namespace Contracts.Tasks;

public record UpdateEssencePriceRequest
{
    public decimal? Price { get; init; }
};