using FanficsWorld.Common.Configurations;
using FanficsWorld.DataAccess;
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
    private readonly IFanficStatusUpdatingService _service;

    public FanficStatusUpdatingJob(IFanficStatusUpdatingService service)
    {
        _service = service;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _service.UpdateFanficsStatusesAsync();
    }
}