using System.Reflection;
using FanficsWorld.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.DataAccess;

public class FanficsDbContext : IdentityDbContext<User>
{
    public DbSet<Fanfic> Fanfics { get; set; }
    
    public DbSet<Fandom> Fandoms { get; set; }
    
    public DbSet<Tag> Tags { get; set; }
    
    public DbSet<Feedback> Feedbacks { get; set; }

    public FanficsDbContext(DbContextOptions<FanficsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}