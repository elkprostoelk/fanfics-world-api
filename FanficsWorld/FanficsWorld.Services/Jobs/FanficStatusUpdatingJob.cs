using FanficsWorld.Services.Interfaces;
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