using FanficsWorld.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FanficsWorld.WebAPI;

public class FanficsDbContextDesignTimeFactory : IDesignTimeDbContextFactory<FanficsDbContext>
{
    public FanficsDbContext CreateDbContext(string[] args)
    {
        var configuration = GetAppConfiguration();
        var optionsBuilder = new DbContextOptionsBuilder<FanficsDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new FanficsDbContext(optionsBuilder.Options);
    }
    
    private static IConfiguration GetAppConfiguration()
    {
        var environmentName =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");

        var dir = Directory.GetParent(AppContext.BaseDirectory);    
        do
            dir = dir.Parent;
        while (dir.Name != "bin");
        dir = dir.Parent;
        var path = dir.FullName;

        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json");

        return builder.Build();
    }
}