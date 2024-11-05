using Microsoft.EntityFrameworkCore;

namespace fintrack.Models;

public class AppDbContext(string connectionString) : DbContext(GetOptions(connectionString))
{
    private static DbContextOptions<AppDbContext> GetOptions(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return optionsBuilder.Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserDbConfig());
    }
}