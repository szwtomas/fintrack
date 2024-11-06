using fintrack.Models;
using Fintrack.Services.Errors;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Services;

public class UserCreationService(AppDbContext db, AuthService authService)
{
    public async Task<User> CreateUser(string email, string password)
    {
        var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
        {
            throw new UserAlreadyExistsException($"User with email {email} already exist");
        }

        var user = new User
        {
            Email = email,
            HashedPassword = authService.HashPassword(password)
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }
}