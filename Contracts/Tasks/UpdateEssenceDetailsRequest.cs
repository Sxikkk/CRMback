namespace Contracts.Tasks;

public record UpdateEssenceDetailsRequest
{
    public string Title { get; init; }
    public string? Description { get; init; }
};