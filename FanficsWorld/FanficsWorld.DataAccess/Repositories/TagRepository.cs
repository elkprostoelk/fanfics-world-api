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

    public async Task<bool> ContainsAllAsync(ICollection<long> ids, CancellationToken cancellationToken)
    {
        var containsAll = true;
        foreach (var id in ids)
        {
            containsAll = containsAll && await _context.Tags.AnyAsync(tag =>
                tag.Id == id, cancellationToken);
        }

        return containsAll;
    }

    public async Task<Tag?> GetAsync(long id) =>
        await _context.Tags.AsNoTracking()
            .Include(t => t.Fanfics)
            .FirstOrDefaultAsync(t => t.Id == id);
}