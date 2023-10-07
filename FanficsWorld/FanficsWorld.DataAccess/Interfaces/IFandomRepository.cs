using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IFandomRepository
{
    Task<ICollection<Fandom>> GetTop10Async();
    IQueryable<Fandom> GetFandoms(string title);
    Task<ICollection<Fandom>> GetRangeAsync(ICollection<long> fandomIds);
    Task<long?> CreateAsync(Fandom fandom);
    Task<Fandom?> GetAsync(long id);
    Task<bool> ContainsAllAsync(ICollection<long> ids, CancellationToken token);
    
}