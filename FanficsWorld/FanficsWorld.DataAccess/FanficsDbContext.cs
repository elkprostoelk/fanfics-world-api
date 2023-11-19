using System.Reflection;
using FanficsWorld.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.DataAccess;

public class FanficsDbContext(DbContextOptions<FanficsDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Fanfic> Fanfics { get; set; }
    
    public DbSet<Fandom> Fandoms { get; set; }
    
    public DbSet<Tag> Tags { get; set; }
    
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}