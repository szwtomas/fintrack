using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fintrack.Models;

public class StockHoldingDbConfig : IEntityTypeConfiguration<StockHolding>
{
    public void Configure(EntityTypeBuilder<StockHolding> builder)
    {
        builder.ToTable("stock_holding", "public");
        builder.HasKey(sh => sh.HoldingId);
        builder.Property(sh => sh.HoldingId).HasColumnName("holding_id").IsRequired();
        builder.Property(sh => sh.Amount).HasColumnName("amount").IsRequired();
        builder.Property(sh => sh.Ticker).HasColumnName("ticker").HasMaxLength(15).IsRequired();
        builder
            .HasOne(sh => sh.Holding)
            .WithOne(sh => sh.StockHolding)
            .HasForeignKey<StockHolding>(sh => sh.HoldingId)
            .IsRequired(false);
    }
}