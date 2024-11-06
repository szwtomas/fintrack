using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fintrack.Models;

public class HoldingDbConfig : IEntityTypeConfiguration<Holding>
{
    public void Configure(EntityTypeBuilder<Holding> builder)
    {
        builder.ToTable("holding", "public");
        builder.HasKey(h => h.HoldingId);
        builder.Property(h => h.HoldingId).HasColumnName("holding_id").IsRequired();
        builder.Property(h => h.HoldingGroupId).HasColumnName("holding_group_id").IsRequired();
        builder.Property(h => h.Type).HasColumnName("type").HasMaxLength(127).IsRequired();
        builder.HasOne(h => h.HoldingGroup)
            .WithMany(hg => hg.Holdings)
            .HasForeignKey(h => h.HoldingGroupId);
    }
}