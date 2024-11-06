namespace fintrack.Models;

public class Holding
{
    public int HoldingId { get; set; }
    public int HoldingGroupId { get; set; }
    public string Type { get; set; } = null!;
    public HoldingGroup HoldingGroup { get; set; } = null!;
    public CashHolding? CashHolding { get; set; }
    public StockHolding? StockHolding { get; set; }
}