using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorld.DataAccess.Entities.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(f => f.Name)
            .IsRequired(false)
            .HasMaxLength(20);

        builder.Property(f => f.Text)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(f => f.Reviewed)
            .IsRequired()
            .HasDefaultValue(false);
    }
}