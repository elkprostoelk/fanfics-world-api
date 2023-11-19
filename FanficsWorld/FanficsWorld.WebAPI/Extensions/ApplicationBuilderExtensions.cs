using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app, IConfiguration configuration, ILogger logger)
    {
        var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var serviceScope = serviceScopeFactory.CreateScope();
        await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<FanficsDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        var userService = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
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
                    logger.LogError("Admin role was not added! Errors: {ErrorsList}", adminRoleAdded.Errors);
                }
                if (!userRoleAdded.Succeeded)
                {
                    logger.LogError("User role was not added! Errors: {ErrorsList}", adminRoleAdded.Errors);
                }
            }
        }

        var adminSettings = configuration.GetSection("AdminSettings")
            .Get<RegisterUserDto>();
        if (await userManager.FindByEmailAsync(adminSettings!.Email) is null)
        {
            var registered = await userService.RegisterUserAsync(adminSettings);
            if (registered is not null)
            {
                if (registered.Succeeded)
                {
                    logger.LogInformation("Admin user was registered!");
                }
                else
                {
                    logger.LogError(
                        "Admin user was not registered! Errors: {ErrorsList}",
                        registered.Errors);
                }
            }
        }
    }

    public static void UseFsExceptionHandler(this WebApplication app, ILogger logger)
    {
        app.UseExceptionHandler(appBuilder => appBuilder.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature!.Error;
            logger.LogError(exception, "An exception occured while processing the request");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "Internal Server Error" });
        }));
    }
}