using FanficsWorld.Common.Enums;
using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IFanficRepository
{
    Task<Fanfic?> GetAsync(long id);
    Task<long?> AddAsync(Fanfic fanfic);
    Task<bool> DeleteAsync(Fanfic fanfic);
    Task<bool> UpdateAsync(Fanfic fanfic);
    IQueryable<Fanfic> GetAllPaged(int pageNumber, int takeCount);
    IQueryable<Fanfic> GetAllInProgress(int takeCount);
    Task<long> CountAsync();
    Task UpdateRangeAsync(List<Fanfic> changedFanfics);
    Task<long> CountByStatusAsync(FanficStatus fanficStatus);
    IQueryable<Fanfic> GetAll();
}