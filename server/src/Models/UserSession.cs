namespace fintrack.Models;

public class UserSession
{
    public string SessionToken { get; set; } = null!;
    public int UserId { get; set; }
    public bool IsActive { get; set; }
    public DateTime ExpiresAt { get; set; }
    public User User { get; set; } = null!;

    private bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }

    public bool IsValid()
    {
        return IsActive && !IsExpired();
    }
}