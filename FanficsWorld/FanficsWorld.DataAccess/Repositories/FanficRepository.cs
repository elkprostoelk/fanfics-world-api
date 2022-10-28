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
            .Include(f => f.Coauthors)
            .Include(f => f.FanficCoauthors)
            .SingleOrDefaultAsync(f => f.Id == id);

    public async Task<bool> AddAsync(Fanfic fanfic)
    {
        fanfic.CreatedDate = DateTime.Now;
        fanfic.LastModified = DateTime.Now;
        await _context.Fanfics.AddAsync(fanfic);
        return await _context.SaveChangesAsync() > 0;
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

    public IQueryable<Fanfic> GetAllInProgressAsync(int skipCount, int takeCount)
    {
        return _context.Fanfics.Skip(skipCount)
                .Take(takeCount)
                .Where(ffic => ffic.Status == FanficStatus.InProgress);
    }

    public async Task UpdateRangeAsync(ICollection<Fanfic> changedFanfics)
    {
        for (var i = 0; i < changedFanfics.Count; i++)
        {
            changedFanfics.ElementAt(i).LastModified = DateTime.Now;
        }
        _context.Fanfics.UpdateRange(changedFanfics);
        await _context.SaveChangesAsync();
    }

    public async Task<long> CountAsync() =>
        await _context.Fanfics.LongCountAsync();
}