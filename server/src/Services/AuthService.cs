using fintrack.Models;
using Fintrack.Services.Errors;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Services;

public class AuthService(AppDbContext db)
{
    public async Task<User?> AuthenticateCredentials(string email, string password)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new UserNotFoundException($"User with email {email} not found");
        }

        var passwordMatch = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
        return passwordMatch ? user : null;
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}