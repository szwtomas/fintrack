using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fintrack.Models;

public class HoldingGroupDailyAggDbConfig : IEntityTypeConfiguration<HoldingGroupDailyAgg>
{
    public void Configure(EntityTypeBuilder<HoldingGroupDailyAgg> builder)
    {
        builder.ToTable("holding_group_agg_daily", "public");
        builder.HasKey(holdingAgg => holdingAgg.HoldingGroupDailyAggId);
        builder
            .Property(holdingAgg => holdingAgg.HoldingGroupDailyAggId)
            .HasColumnName("holding_group_agg_daily_id")
            .IsRequired();

        builder
            .Property(holdingAgg => holdingAgg.HoldingGroupId)
            .HasColumnName("holding_group_id").IsRequired();

        builder.Property(holdingAgg => holdingAgg.Value).HasColumnName("value").IsRequired();
        builder.Property(holdingAgg => holdingAgg.Date).HasColumnName("date").IsRequired();
        builder.Property(holdingAgg => holdingAgg.Currency).HasColumnName("currency").IsRequired();

        builder
            .HasOne(holdingAgg => holdingAgg.HoldingGroup)
            .WithMany(hg => hg.DailyAggregates)
            .HasForeignKey(holdingAgg => holdingAgg.HoldingGroupId);

        builder
            .HasIndex(agg => new { agg.HoldingGroupId })
            .HasDatabaseName("idx_holding_group_agg_daily_holding_group_id");
    }
}