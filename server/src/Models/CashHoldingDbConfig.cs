using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fintrack.Models;

public class CashHoldingDbConfig : IEntityTypeConfiguration<CashHolding>
{
    public void Configure(EntityTypeBuilder<CashHolding> builder)
    {
        builder.ToTable("cash_holding", "public");
        builder.HasKey(ch => ch.HoldingId);
        builder.Property(ch => ch.HoldingId).HasColumnName("holding_id").IsRequired();
        builder.Property(ch => ch.Currency).HasColumnName("currency").HasMaxLength(63).IsRequired();
        builder.Property(ch => ch.Value).HasColumnName("value").IsRequired();

        builder
            .HasOne(ch => ch.Holding)
            .WithOne(h => h.CashHolding)
            .HasForeignKey<CashHolding>(ch => ch.HoldingId)
            .IsRequired(false);
    }
}