using FanficsWorld.Common.Enums;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.DataAccess.Repositories;

public class FanficRepository : IFanficRepository
{
    private readonly FanficsDbContext _context;

    public FanficRepository(FanficsDbContext context)
    {
        _context = context;
    }

    public async Task<Fanfic?> GetAsync(long id) =>
        await _context.Fanfics
            .Include(f => f.Author)
            .Include(f => f.Coauthors.Where(fc => !fc.IsBlocked))
            .Include(f => f.FanficCoauthors)
            .Include(f => f.Fandoms)
            .Include(f => f.FanficFandoms)
            .Include(f => f.Tags)
            .Include(f => f.FanficTags)
            .FirstOrDefaultAsync(f => f.Id == id);

    public async Task<long?> AddAsync(Fanfic fanfic)
    {
        fanfic.LastModified = DateTime.Now;
        await _context.Fanfics.AddAsync(fanfic);
        return await _context.SaveChangesAsync() > 0 ? fanfic.Id : null;
    }

    public async Task<bool> DeleteAsync(Fanfic fanfic)
    {
        _context.Fanfics.Remove(fanfic);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Fanfic fanfic)
    {
        fanfic.LastModified = DateTime.Now;
        _context.Fanfics.Update(fanfic);
        return await _context.SaveChangesAsync() > 0;
    }

    public IQueryable<Fanfic> GetAll() =>
        _context.Fanfics.AsNoTracking()
            .Include(ffic => ffic.Author)
            .Include(ffic => ffic.Coauthors.Where(c => !c.IsBlocked))
            .Include(ffic => ffic.Fandoms)
            .Include(ffic => ffic.Tags)
            .Where(f => !f.Author!.IsBlocked);

    public IQueryable<Fanfic> GetAllPaged(int pageNumber, int takeCount) =>
        _context.Fanfics
            .AsNoTracking()
            .Skip((pageNumber - 1) * takeCount)
            .Take(takeCount)
            .Include(ffic => ffic.Author)
            .Include(ffic => ffic.Coauthors.Where(c => !c.IsBlocked))
            .Include(ffic => ffic.Fandoms)
            .Include(ffic => ffic.Tags)
            .Where(f => !f.Author!.IsBlocked);
    
    public IQueryable<Fanfic> GetAllInProgress(int takeCount) =>
        _context.Fanfics.AsNoTracking()
            .Take(takeCount)
            .Where(ffic => ffic.Status == FanficStatus.InProgress);

    public async Task UpdateRangeAsync(List<Fanfic> changedFanfics)
    {
        for (var i = 0; i < changedFanfics.Count; i++)
        {
            changedFanfics.ElementAt(i).LastModified = DateTime.Now;
        }
        _context.Fanfics.UpdateRange(changedFanfics);
        await _context.SaveChangesAsync();
    }

    public async Task<long> CountByStatusAsync(FanficStatus fanficStatus) =>
        await _context.Fanfics.LongCountAsync(ffic => ffic.Status == fanficStatus);

    public async Task<long> CountAsync() =>
        await _context.Fanfics.LongCountAsync();
}