using Microsoft.EntityFrameworkCore;

namespace fintrack.Models;

public class AppDbContext(string connectionString) : DbContext(GetOptions(connectionString))
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<HoldingGroup> HoldingGroups { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    public DbSet<CashHolding> CashHoldings { get; set; }
    public DbSet<StockHolding> StockHoldings { get; set; }
    public DbSet<HoldingGroupDailyAgg> HoldingGroupDailyAggs { get; set; }

    private static DbContextOptions<AppDbContext> GetOptions(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return optionsBuilder.Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserDbConfig());
        modelBuilder.ApplyConfiguration(new HoldingGroupDbConfig());
        modelBuilder.ApplyConfiguration(new HoldingDbConfig());
        modelBuilder.ApplyConfiguration(new CashHoldingDbConfig());
        modelBuilder.ApplyConfiguration(new StockHoldingDbConfig());
        modelBuilder.ApplyConfiguration(new HoldingGroupDailyAggDbConfig());
        modelBuilder.ApplyConfiguration(new UserSessionDbConfig());
    }
}