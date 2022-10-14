using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface ITagRepository
{
    Task<ICollection<Tag>> GetAllAsync();
    Task<ICollection<Tag>> GetTop10Async();
    Task<ICollection<Tag>> GetRangeAsync(ICollection<long> tagIds);
    Task<bool> ContainsAllAsync(ICollection<long> ids, CancellationToken cancellationToken);
}