using FanficsWorld.Common.Configurations;
using FanficsWorld.Services.Interfaces;
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
        var fanficStatusUpdatingConfig =
            scope.ServiceProvider.GetRequiredService<IOptions<FanficStatusUpdatingConfiguration>>();
        var fanficService = scope.ServiceProvider.GetRequiredService<IFanficService>();
        var service = new FanficsStatusUpdatingService(logger, fanficStatusUpdatingConfig, fanficService);
        await service.UpdateFanficsStatusesAsync();
    }
}