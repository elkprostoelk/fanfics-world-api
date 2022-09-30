using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorld.DataAccess.Entities.Configurations;

public class FanficConfiguration : IEntityTypeConfiguration<Fanfic>
{
    public void Configure(EntityTypeBuilder<Fanfic> builder)
    {
        builder.ToTable("Fanfics");
        
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(f => f.Annotation)
            .HasMaxLength(100);

        builder.Property(f => f.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(f => f.Author)
            .WithMany(u => u.Fanfics)
            .HasForeignKey(f => f.AuthorId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(f => f.Coauthors)
            .WithMany(u => u.CoauthoredFanfics)
            .UsingEntity<FanficCoauthor>(x => 
                    x.HasOne(fc => fc.Coauthor)
                        .WithMany(u => u.FanficCoauthors)
                        .HasForeignKey(fc => fc.CoauthorId)
                        .OnDelete(DeleteBehavior.Restrict), 
                x => x.HasOne(fc => fc.Fanfic)
                        .WithMany(f => f.FanficCoauthors)
                        .HasForeignKey(fc => fc.FanficId)
                        .OnDelete(DeleteBehavior.Restrict),
                x =>
                {
                    x.HasKey(fc => new {fc.FanficId, fc.CoauthorId});
                    x.ToTable("FanficCoauthors");
                });
    }
}