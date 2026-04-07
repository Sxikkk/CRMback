namespace Contracts.Tasks;

public record UpdateEssenceDueDateRequest
{
    public DateTime DueDate { get; init; }
}