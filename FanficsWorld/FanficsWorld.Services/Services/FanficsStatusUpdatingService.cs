using FanficsWorld.Common.Configurations;
using FanficsWorld.Common.Enums;
using FanficsWorld.DataAccess;
using FanficsWorld.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FanficsWorld.Services.Services;

public class FanficsStatusUpdatingService : IFanficStatusUpdatingService
{
    private readonly ILogger<FanficsStatusUpdatingService> _logger;
    private readonly FanficsDbContext _dbContext;
    private readonly int _fanficFrozenAfterDays;

    public FanficsStatusUpdatingService(
        ILogger<FanficsStatusUpdatingService> logger,
        FanficsDbContext dbContext,
        IOptions<FanficStatusUpdatingConfiguration> fanficStatusUpdatingOptions)
    {
        _logger = logger;
        _dbContext = dbContext;
        _fanficFrozenAfterDays = fanficStatusUpdatingOptions.Value.FanficFrozenAfterDays;
    }

    public async Task UpdateFanficsStatusesAsync()
    {
        _logger.LogInformation("Starting of fanfic freezer service");

        var affectedFanficsCount = await _dbContext
            .Fanfics
            .Where(f => f.Status == FanficStatus.InProgress
                && EF.Functions.DateDiffDay(f.LastModified.Date, DateTime.Now.Date) >= _fanficFrozenAfterDays)
            .ExecuteUpdateAsync(
                f => f.SetProperty(fanfic => fanfic.Status,
                (_) => FanficStatus.Frozen));

        _logger.LogInformation("{AffectedFanficsCount} fanfic(s) frozen", affectedFanficsCount);
        _logger.LogInformation("Fanfic freezer service has finished the task");
    }
}