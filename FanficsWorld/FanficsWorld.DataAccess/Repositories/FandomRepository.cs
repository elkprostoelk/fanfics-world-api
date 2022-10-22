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

    public async Task<ICollection<Fandom>> GetTop10Async() =>
        await _context.Fandoms
            .Include(fdom => fdom.Fanfics)
            .OrderByDescending(fdom => fdom.Fanfics.Count)
            .ThenBy(fdom => fdom.Title)
            .AsNoTracking()
            .Take(10).ToListAsync();

    public async Task<ICollection<Fandom>> GetRangeAsync(ICollection<long> fandomIds) =>
        await _context.Fandoms
            .AsNoTracking()
            .Where(fdom => fandomIds.Contains(fdom.Id)).ToListAsync();

    public async Task<bool> CreateAsync(Fandom fandom)
    {
        await _context.Fandoms.AddAsync(fandom);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Fandom?> GetAsync(long id) =>
        await _context.Fandoms
            .Include(fdom => fdom.Fanfics)
            .AsNoTracking()
            .FirstOrDefaultAsync(fdom => fdom.Id == id);
}