using fintrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Services;

public class SessionService(AppDbContext db)
{
    private const int SessionDurationDays = 15;

    public async Task<UserSession?> GetSession(string? sessionToken)
    {
        if (sessionToken == null)
        {
            return null;
        }

        return await db.UserSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(us => us.SessionToken.Equals(sessionToken));
    }

    public async Task<string> CreateSession(int userId)
    {
        var token = Guid.NewGuid().ToString();
        var session = new UserSession
        {
            SessionToken = token,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromDays(SessionDurationDays)),
            IsActive = true
        };

        db.UserSessions.Add(session);
        await db.SaveChangesAsync();
        return token;
    }

    public async Task<bool> DeleteSession(string sessionToken)
    {
        var session = await GetSession(sessionToken);
        if (session == null)
        {
            return false;
        }

        db.UserSessions.Remove(session);
        await db.SaveChangesAsync();
        return true;
    }
}