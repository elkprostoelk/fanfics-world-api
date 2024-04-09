using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorld.DataAccess.Entities.Configurations;

public class FanficCommentConfiguration : IEntityTypeConfiguration<FanficComment>
{
    public void Configure(EntityTypeBuilder<FanficComment> builder)
    {
        builder.HasKey(fc => fc.Id);

        builder.Property(fc => fc.CreatedDate)
            .IsRequired();

        builder.Property(fc => fc.Text)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(fc => fc.Reactions)
            .WithOne(r => r.FanficComment)
            .HasForeignKey(r => r.FanficCommentId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}