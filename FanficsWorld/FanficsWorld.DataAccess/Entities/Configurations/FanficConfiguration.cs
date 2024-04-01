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

        builder.Property(f => f.LastModified)
            .IsRequired();

        builder.Property(ffic => ffic.Views)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(f => f.Author)
            .WithMany(u => u.Fanfics)
            .HasForeignKey(f => f.AuthorId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(f => f.Coauthors)
            .WithMany(u => u.CoauthoredFanfics)
            .UsingEntity<FanficCoauthor>(x => 
                    x.HasOne(fc => fc.Coauthor)
                        .WithMany(u => u.FanficCoauthors)
                        .HasForeignKey(fc => fc.CoauthorId), 
                x => x.HasOne(fc => fc.Fanfic)
                        .WithMany(f => f.FanficCoauthors)
                        .HasForeignKey(fc => fc.FanficId),
                x =>
                {
                    x.HasKey(fc => new {fc.FanficId, fc.CoauthorId});
                    x.ToTable("FanficCoauthors");
                });

        builder.HasMany(ffic => ffic.Fandoms)
            .WithMany(fdom => fdom.Fanfics)
            .UsingEntity<FanficFandom>(
                ffBuilder => 
                    ffBuilder.HasOne(ff => ff.Fandom)
                        .WithMany(ffic => ffic.FanficFandoms)
                        .HasForeignKey(ff => ff.FandomId),
                ffBuilder => ffBuilder.HasOne(ff => ff.Fanfic)
                    .WithMany(fdom => fdom.FanficFandoms)
                    .HasForeignKey(ff => ff.FanficId),
                ffBuilder =>
                {
                    ffBuilder.HasKey(ff => new {ff.FandomId, ff.FanficId});
                    ffBuilder.ToTable("FanficFandoms");
                });

        builder.HasMany(ffic => ffic.Tags)
            .WithMany(t => t.Fanfics)
            .UsingEntity<FanficTag>(
                ftBuilder =>
                    ftBuilder.HasOne(ft => ft.Tag)
                    .WithMany(t => t.FanficTags)
                    .HasForeignKey(ft => ft.TagId),
                ftBuilder =>
                    ftBuilder.HasOne(ft => ft.Fanfic)
                    .WithMany(ffic => ffic.FanficTags)
                    .HasForeignKey(ft => ft.FanficId),
                ftBuilder =>
                {
                    ftBuilder.HasKey(ft => new {ft.FanficId, ft.TagId});
                    ftBuilder.ToTable("FanficTags");
                });
    }
}