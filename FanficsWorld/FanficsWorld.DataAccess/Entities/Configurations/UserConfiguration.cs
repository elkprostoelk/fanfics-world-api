using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorld.DataAccess.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(u => u.PhoneNumberConfirmed)
            .Ignore(u => u.TwoFactorEnabled);

        builder.Property(u => u.Age)
            .IsRequired();

        builder.Property(u => u.RegistrationDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);
        
        builder.Property(u => u.UserName)
            .HasMaxLength(20);
    }
}