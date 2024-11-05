using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fintrack.Models;

public class UserSessionDbConfig : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("user_session", "public");
        builder.HasKey(us => us.SessionToken);
        builder.Property(us => us.SessionToken).HasColumnName("session_token").HasMaxLength(63).IsRequired();
        builder.Property(us => us.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(us => us.ExpiresAt).HasColumnName("expires_at").IsRequired();
        builder.Property(us => us.IsActive).HasColumnName("is_active").IsRequired();

        builder.HasOne(us => us.User).WithMany(u => u.Sessions).HasForeignKey(us => us.UserId);
    }
}