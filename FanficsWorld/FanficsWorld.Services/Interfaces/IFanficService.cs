using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFanficService
{
    Task<FanficDto?> GetByIdAsync(long id);
    Task<long?> CreateAsync(NewFanficDto newFanficDto, string userId);
    Task<bool> DeleteAsync(long id);
    Task<bool> AddTagsToFanficAsync(long fanficId, AddTagsDto addTagsDto);
    Task UpdateFanficsStatusesAsync();
    Task<ulong?> IncrementFanficViewsCounterAsync(long fanficId);
    Task<ICollection<SimpleFanficDto>> GetPageWithFanficsAsync(int page, int itemsPerPage);
}