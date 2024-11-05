using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fintrack.Models;

public class UserDbConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user", "public");
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(127).IsRequired();
        builder.Property(u => u.HashedPassword).HasColumnName("password").HasMaxLength(127).IsRequired();
    }
}