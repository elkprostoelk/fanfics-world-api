using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFanficService
{
    Task<FanficDto?> GetByIdAsync(long id);
    Task<bool> CreateAsync(NewFanficDto newFanficDto, string userId);
    Task<bool> DeleteAsync(long id);
    Task<bool> AddTagsToFanficAsync(long fanficId, AddTagsDto addTagsDto);
}