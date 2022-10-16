using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface ITagService
{
    Task<ICollection<TagDto>?> GetAllAsync();
    Task<ICollection<TagDto>?> GetTop10Async();
    Task<bool> ContainsAllAsync(ICollection<long> ids, CancellationToken cancellationToken);
    Task<TagWithFanficsDto?> GetFullByIdAsync(long id);
}