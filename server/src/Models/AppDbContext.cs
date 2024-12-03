using Microsoft.EntityFrameworkCore;

namespace fintrack.Models;

public class AppDbContext() : DbContext(GetOptions())
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<HoldingGroup> HoldingGroups { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    public DbSet<CashHolding> CashHoldings { get; set; }
    public DbSet<StockHolding> StockHoldings { get; set; }
    public DbSet<HoldingGroupDailyAgg> HoldingGroupDailyAggs { get; set; }

    private static DbContextOptions<AppDbContext> GetOptions()
    {
        const string connectionString =
            "Host=localhost;Database=postgres;Username=postgres;Password=postgres;Port=5435;";
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