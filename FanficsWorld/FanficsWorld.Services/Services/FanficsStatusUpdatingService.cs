using FanficsWorld.Common.Configurations;
using FanficsWorld.Common.Enums;
using FanficsWorld.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FanficsWorld.Services.Services;

public class FanficsStatusUpdatingService(
    ILogger<FanficsStatusUpdatingService> logger,
    IOptions<FanficStatusUpdatingConfiguration> fanficStatusUpdatingConfig,
    IFanficService fanficService
        )
{
    private readonly ILogger<FanficsStatusUpdatingService> _logger = logger;
    private readonly IFanficService _fanficService = fanficService;
    private readonly FanficStatusUpdatingConfiguration _fanficStatusUpdatingConfig = fanficStatusUpdatingConfig.Value;

    public async Task UpdateFanficsStatusesAsync()
    {
        try
        {
            var fanficsCount = await _fanficService.CountInProgressAsync();
            var chunksCount = Convert.ToInt32(
                Math.Ceiling(fanficsCount / (double)_fanficStatusUpdatingConfig.ChunkSize));
            for (var i = 0; i < chunksCount; i++)
            {
                var chunk = await _fanficService.GetMinifiedInProgressAsync(_fanficStatusUpdatingConfig.ChunkSize);
                for (var j = 0; j < _fanficStatusUpdatingConfig.ChunkSize; j++)
                {
                    var difference = (DateTime.Now - chunk.ElementAt(j).LastModified).Days;
                    if (difference >= _fanficStatusUpdatingConfig.FanficFrozenAfterDays)
                    {
                        await _fanficService.SetFanficStatusAsync(chunk.ElementAt(j).Id, FanficStatus.Frozen);
                    }
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
        }
    }
}