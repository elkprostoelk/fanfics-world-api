using FanficsWorld.DataAccess;
using FanficsWorld.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.WebAPI.Extensions;

public static class ServiceBuilderExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FanficsDbContext>(x =>
            x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = false;
                opts.User.RequireUniqueEmail = true;
                opts.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<FanficsDbContext>()
            .AddDefaultTokenProviders();
    }
}