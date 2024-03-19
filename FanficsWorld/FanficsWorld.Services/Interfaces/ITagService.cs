using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface ITagService
{
    Task<List<TagDto>?> GetAllAsync(string? title = null);
    Task<List<TagDto>?> GetTop10Async();
    Task<bool> ContainsAllAsync(List<long> ids, CancellationToken cancellationToken);
    Task<TagWithFanficsDto?> GetFullByIdAsync(long id);
}