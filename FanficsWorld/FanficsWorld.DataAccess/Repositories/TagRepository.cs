﻿using FanficsWorld.DataAccess.Entities;
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

    public IQueryable<Tag> GetAll() =>
        _context.Tags
            .AsNoTracking()
            .Include(t => t.Fanfics);

    public async Task<List<Tag>> GetTop10Async() =>
        await _context.Tags
            .Include(t => t.Fanfics)
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.Fanfics.Count)
            .Take(10).ToListAsync();

    public async Task<List<Tag>> GetRangeAsync(List<long> tagIds) =>
        await _context.Tags
            .Where(t => tagIds.Contains(t.Id))
            .ToListAsync();

    public async Task<bool> ContainsAllAsync(List<long> ids, CancellationToken cancellationToken)
    {
        var containsAll = true;
        foreach (var id in ids)
        {
            containsAll = containsAll 
                          && await _context
                              .Tags
                              .AnyAsync(
                                  tag => !tag.IsDeleted && tag.Id == id,
                                  cancellationToken);
        }

        return containsAll;
    }

    public async Task<Tag?> GetAsync(long id) =>
        await _context.Tags.AsNoTracking()
            .Include(t => t.Fanfics)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<bool> AddAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Tag tag)
    {
        tag.IsDeleted = true;
        _context.Tags.Update(tag);
        return await _context.SaveChangesAsync() > 0;
    }
}