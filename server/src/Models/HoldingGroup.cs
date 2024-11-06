namespace fintrack.Models;

public class HoldingGroup
{
    public int HoldingGroupId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<Holding> Holdings { get; set; } = null!;
    public ICollection<HoldingGroupDailyAgg> DailyAggregates { get; set; } = null!;
}