using FanficsWorld.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace FanficsWorld.Services.Jobs;

[DisallowConcurrentExecution]
public class FanficStatusUpdatingJob : IJob
{
    private readonly IFanficService _fanficService;
    private readonly ILogger<FanficStatusUpdatingJob> _logger;

    public FanficStatusUpdatingJob(
        IFanficService fanficService,
        ILogger<FanficStatusUpdatingJob> logger)
    {
        _fanficService = fanficService;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _fanficService.UpdateFanficsStatusesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception occured while processing the job");
        }
    }
}