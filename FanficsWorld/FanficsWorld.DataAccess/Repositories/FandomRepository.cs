using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.DataAccess.Repositories;

public class FandomRepository : IFandomRepository
{
    private readonly FanficsDbContext _context;

    public FandomRepository(FanficsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Fandom>> GetTop10Async() =>
        await _context.Fandoms
            .Include(fdom => fdom.Fanfics)
            .Where(f => !f.IsDeleted)
            .OrderByDescending(fdom => fdom.Fanfics.Count)
            .ThenBy(fdom => fdom.Title)
            .AsNoTracking()
            .Take(10)
            .ToListAsync();

    public IQueryable<Fandom> GetFandoms(string title) =>
        _context.Fandoms
            .AsNoTracking()
            .Where(fdom => !fdom.IsDeleted && fdom.Title.Contains(title))
            .Take(10);

    public async Task<List<Fandom>> GetRangeAsync(List<long> fandomIds) =>
        await _context.Fandoms
            .Where(fdom => !fdom.IsDeleted && fandomIds.Contains(fdom.Id))
            .ToListAsync();

    public async Task<long?> CreateAsync(Fandom fandom)
    {
        await _context.Fandoms.AddAsync(fandom);
        return await _context.SaveChangesAsync() > 0 ? fandom.Id : null;
    }

    public async Task<Fandom?> GetAsync(long id) =>
        await _context.Fandoms
            .Include(fdom => fdom.Fanfics)
            .Include(fdom => fdom.FanficFandoms)
            .AsNoTracking()
            .FirstOrDefaultAsync(fdom => fdom.Id == id);

    public async Task<bool> ContainsAllAsync(List<long> ids, CancellationToken token)
    {
        var containsAll = true;
        foreach (var id in ids)
        {
            containsAll = containsAll && await _context.Fandoms.AnyAsync(f =>
                !f.IsDeleted && f.Id == id, token);
        }

        return containsAll;
    }

    public IQueryable<Fandom> GetAll() =>
        _context.Fandoms
            .AsNoTracking()
            .Include(f => f.Fanfics)
            .AsQueryable();

    public async Task<bool> DeleteAsync(Fandom fandom)
    {
        fandom.IsDeleted = true;
        _context.Fandoms.Update(fandom);
        return await _context.SaveChangesAsync() > 0;
    }
}