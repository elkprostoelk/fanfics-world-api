using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface ITagService
{
    Task<ICollection<TagDto>?> GetAllAsync();
    Task<ICollection<TagDto>?> GetTop10Async();
}