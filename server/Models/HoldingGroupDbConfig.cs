using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fintrack.Models;

public class HoldingGroupDbConfig : IEntityTypeConfiguration<HoldingGroup>
{
    public void Configure(EntityTypeBuilder<HoldingGroup> builder)
    {
        builder.ToTable("holding_group", "public");
        builder.HasKey(hg => hg.HoldingGroupId);
        builder.Property(hg => hg.HoldingGroupId).HasColumnName("holding_group_id").IsRequired();
        builder.Property(hg => hg.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(hg => hg.Name).HasColumnName("name").HasMaxLength(127).IsRequired();
        builder.HasOne(hg => hg.User).WithMany(u => u.HoldingGroups).HasForeignKey(hg => hg.UserId);
    }
}