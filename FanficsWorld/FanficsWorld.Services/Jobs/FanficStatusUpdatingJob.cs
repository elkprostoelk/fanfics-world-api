using FanficsWorld.Common.Configurations;
using FanficsWorld.DataAccess;
using FanficsWorld.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace FanficsWorld.Services.Jobs;

[DisallowConcurrentExecution]
public class FanficStatusUpdatingJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FanficStatusUpdatingJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FanficsStatusUpdatingService>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<FanficsDbContext>();
        var fanficUpdatingOptions = scope.ServiceProvider.GetRequiredService<IOptions<FanficStatusUpdatingConfiguration>>();
        var service = new FanficsStatusUpdatingService(logger, dbContext, fanficUpdatingOptions);
        await service.UpdateFanficsStatusesAsync();
    }
}