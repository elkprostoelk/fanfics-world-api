using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorld.DataAccess.Entities.Configurations;

public class FandomConfiguration : IEntityTypeConfiguration<Fandom>
{
    public void Configure(EntityTypeBuilder<Fandom> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
    }
}