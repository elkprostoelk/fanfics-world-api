using FanficsWorld.Common.DTO;
using FanficsWorld.Common.Enums;

namespace FanficsWorld.Services.Interfaces;

public interface IFanficService
{
    Task<FanficDto?> GetByIdAsync(long id);
    Task<long?> CreateAsync(NewFanficDto newFanficDto, string userId);
    Task<bool> DeleteAsync(long id);
    Task<bool> AddTagsToFanficAsync(long fanficId, AddTagsDto addTagsDto);
    Task<ulong?> IncrementFanficViewsCounterAsync(long fanficId);
    Task<ServicePagedResultDto<SimpleFanficDto>> GetPageWithFanficsAsync(int page, int itemsPerPage);
    Task<long> CountInProgressAsync();
    Task<ICollection<MinifiedFanficDto>> GetMinifiedInProgressAsync(int chunkSize);
    Task SetFanficStatusAsync(long id, FanficStatus fanficStatus);
}