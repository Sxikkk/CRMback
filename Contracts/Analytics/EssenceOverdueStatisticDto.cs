namespace Contracts.Analytics;

public record EssenceOverdueStatisticDto
{
    public int Total { get; set; }
    public int Overdue { get; set; }
    public decimal OverduePercent { get; set; }
    public int OverdueNormal { get; set; }
    public int OverdueWarning { get; set; }
    public int OverdueDanger { get; set; }
}
