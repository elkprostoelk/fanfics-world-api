using FanficsWorld.Common.Configurations;
using FanficsWorld.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FanficsWorld.Services.Services;

public class FanficsStatusUpdatingService
{
    private readonly ILogger<FanficsStatusUpdatingService> _logger;
    private readonly FanficsDbContext _dbContext;
    private readonly FanficStatusUpdatingConfiguration _fanficStatusUpdatingOptions;

    public FanficsStatusUpdatingService(
        ILogger<FanficsStatusUpdatingService> logger,
        FanficsDbContext dbContext,
        IOptions<FanficStatusUpdatingConfiguration> fanficStatusUpdatingOptions)
    {
        _logger = logger;
        _dbContext = dbContext;
        _fanficStatusUpdatingOptions = fanficStatusUpdatingOptions.Value;
    }

    public async Task UpdateFanficsStatusesAsync()
    {
        try
        {
            var parameter = new SqlParameter("@FanficFrozenAfterDays",
                _fanficStatusUpdatingOptions.FanficFrozenAfterDays);
            
            var affectedFanficsCount = await _dbContext.Database.ExecuteSqlRawAsync("sp_UpdateFanficStatus @FanficFrozenAfterDays", parameter);
            _logger.LogInformation("{AffectedFanficsCount} fanfics frozen", affectedFanficsCount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            throw;
        }
    }
}