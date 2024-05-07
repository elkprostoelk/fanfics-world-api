using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface ITagRepository
{
    IQueryable<Tag> GetAll();
    Task<List<Tag>> GetTop10Async();
    Task<List<Tag>> GetRangeAsync(List<long> tagIds);
    Task<bool> ContainsAllAsync(List<long> ids, CancellationToken cancellationToken);
    Task<Tag?> GetAsync(long id);
    Task<bool> AddAsync(Tag tag);
    Task<bool> DeleteAsync(Tag tag);
}