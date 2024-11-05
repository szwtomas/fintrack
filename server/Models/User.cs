namespace fintrack.Models;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public ICollection<HoldingGroup> HoldingGroups { get; set; } = null!;
}