namespace fintrack.Models;

public class HoldingGroupDailyAgg
{
    public int HoldingGroupDailyAggId { get; set; }
    public int HoldingGroupId { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public string Currency { get; set; } = null!;
    public HoldingGroup HoldingGroup { get; set; } = null!;
}