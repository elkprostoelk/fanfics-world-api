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

    public async Task<ICollection<Tag>> GetRangeAsync(ICollection<long> tagIds) =>
        await _context.Tags.AsNoTracking()
            .Where(t => tagIds.Contains(t.Id)).ToListAsync();

    public async Task<bool> ContainsAllAsync(ICollection<long> ids, CancellationToken cancellationToken) =>
        await _context.Tags.AllAsync(t => ids.Contains(t.Id), cancellationToken);
}