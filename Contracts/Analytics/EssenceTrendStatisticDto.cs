namespace Contracts.Analytics;

public record EssenceTrendPointDto
{
    public DateTime PeriodStartUtc { get; set; }
    public int Created { get; set; }
    public int Completed { get; set; }
}

public record EssenceTrendStatisticDto
{
    public IReadOnlyList<EssenceTrendPointDto> Points { get; set; } = [];
}
