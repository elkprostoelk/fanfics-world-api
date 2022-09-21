using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app, IConfiguration configuration)
    {
        var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var serviceScope = serviceScopeFactory.CreateScope();
        await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<FanficsDbContext>();
        if (dbContext is not null)
        {
            await dbContext.Database.EnsureCreatedAsync();
            var userService = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = app.Logger;
            if (!await roleManager.Roles.AnyAsync())
            {
                var adminRoleAdded = await roleManager.CreateAsync(new IdentityRole("Admin"));
                var userRoleAdded = await roleManager.CreateAsync(new IdentityRole("User"));
                if (adminRoleAdded.Succeeded && userRoleAdded.Succeeded)
                {
                    logger.LogInformation("Admin, User roles were added!");
                }
                else
                {
                    if (!adminRoleAdded.Succeeded)
                    {
                        logger.LogError("Admin role was not added!");
                    }
                    if (!userRoleAdded.Succeeded)
                    {
                        logger.LogError("User role was not added!");
                    }
                }
            }
            if (await userManager.FindByEmailAsync("admin@admin.com") is null)
            {
                var registered = await userService.RegisterUserAsync(new RegisterUserDTO
                {
                    UserName = "admin",
                    Password = configuration.GetValue<string>("AdminPassword"),
                    Email = "admin@admin.com",
                    Age = 18,
                    PhoneNumber = "+380000000000",
                    Role = "Admin"
                });
                if (registered)
                {
                    logger.LogInformation("Admin user was registered!");
                }
                else
                {
                    logger.LogError("Admin user was not registered!");
                }
            }
        }
    }
}