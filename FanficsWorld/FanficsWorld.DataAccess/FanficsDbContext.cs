using System.Reflection;
using FanficsWorld.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.DataAccess;

public class FanficsDbContext : IdentityDbContext<User>
{
    public FanficsDbContext(DbContextOptions<FanficsDbContext> options) : base(options)
    {
    }

    public DbSet<Fanfic> Fanfics { get; set; }
    
    public DbSet<Fandom> Fandoms { get; set; }
    
    public DbSet<Tag> Tags { get; set; }
    
    public DbSet<Feedback> Feedbacks { get; set; }
    
    public DbSet<FanficComment> FanficComments { get; set; }
    
    public DbSet<FanficCommentReaction> FanficCommentReactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}