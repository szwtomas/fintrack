namespace fintrack.Models;

public class CashHolding
{
    public int HoldingId { get; set; }
    public string Currency { get; set; } = null!;
    public double Value { get; set; }
    
    public Holding Holding { get; set; } = null!;
}
