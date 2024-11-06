namespace fintrack.Models;

public class StockHolding
{
    public int HoldingId { get; set; }
    public int Amount { get; set; }
    public string Ticker { get; set; } = null!;
    public Holding Holding { get; set; } = null!;
}