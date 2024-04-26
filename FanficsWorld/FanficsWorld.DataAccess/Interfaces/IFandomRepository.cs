using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IFandomRepository
{
    Task<List<Fandom>> GetTop10Async();
    IQueryable<Fandom> GetFandoms(string title);
    Task<List<Fandom>> GetRangeAsync(List<long> fandomIds);
    Task<long?> CreateAsync(Fandom fandom);
    Task<Fandom?> GetAsync(long id);
    Task<bool> ContainsAllAsync(List<long> ids, CancellationToken token);
    IQueryable<Fandom> GetAll();
}