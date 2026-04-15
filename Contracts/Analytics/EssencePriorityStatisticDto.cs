namespace Contracts.Analytics;

public record EssencePriorityStatisticDto
{
    public int Normal { get; set; }
    public int Warning { get; set; }
    public int Danger { get; set; }
}
