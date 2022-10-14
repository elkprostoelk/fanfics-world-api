using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.DataAccess.Repositories;

public class TagRepository : ITagRepository
{
    private readonly FanficsDbContext _context;

    public TagRepository(FanficsDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Tag>> GetAllAsync() =>
        await _context.Tags.AsNoTracking().ToListAsync();

    public async Task<ICollection<Tag>> GetTop10Async() =>
        await _context.Tags
            .Include(t => t.Fanfics)
            .AsNoTracking()
            .OrderByDescending(t => t.Fanfics.Count)
            .Take(10).ToListAsync();
}