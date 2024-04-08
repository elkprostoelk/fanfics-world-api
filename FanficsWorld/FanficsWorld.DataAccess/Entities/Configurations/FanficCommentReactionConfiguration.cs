using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorld.DataAccess.Entities.Configurations;

public class FanficCommentReactionConfiguration: IEntityTypeConfiguration<FanficCommentReaction>
{
    public void Configure(EntityTypeBuilder<FanficCommentReaction> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.IsLike)
            .IsRequired();
    }
}