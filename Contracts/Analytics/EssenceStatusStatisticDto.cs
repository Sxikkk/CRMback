using Domain.Enums;

namespace Contracts.Analytics;

public record EssenceStatusStatisticDto
{
    public int Waiting { get; set; }
    public int Paused { get; set; }
    public int InProgress { get; set; }
    public int Completed { get; set; }
};