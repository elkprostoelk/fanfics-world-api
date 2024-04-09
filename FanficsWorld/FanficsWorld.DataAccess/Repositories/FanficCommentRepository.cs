using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.DataAccess.Repositories;

public class FanficCommentRepository : IFanficCommentRepository
{
    private readonly FanficsDbContext _context;
    
    public FanficCommentRepository(FanficsDbContext context) => _context = context;

    public async Task<List<FanficComment>> GetCommentsAsync(long fanficId) =>
        await _context
            .FanficComments
            .AsNoTracking()
            .Where(fc => fc.FanficId == fanficId)
            .Include(fc => fc.Fanfic)
            .Include(fc => fc.Author)
            .Include(fc => fc.Reactions)
            .ToListAsync();

    public async Task<bool> AddCommentAsync(FanficComment comment)
    {
        await _context.FanficComments.AddAsync(comment);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<FanficComment?> GetAsync(long commentId, bool asNoTracking = true)
    {
        var query = _context
            .FanficComments
            .Include(fc => fc.Reactions)
            .AsQueryable();
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(fc => fc.Id == commentId);
    }

    public async Task<bool> RemoveCommentReactionAsync(FanficCommentReaction fanficCommentReaction)
    {
        _context.FanficCommentReactions.Remove(fanficCommentReaction);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddCommentReactionAsync(FanficCommentReaction reaction)
    {
        await _context.FanficCommentReactions.AddAsync(reaction);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateCommentReactionAsync(FanficCommentReaction fanficCommentReaction)
    {
        _context.FanficCommentReactions.Update(fanficCommentReaction);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(FanficComment comment)
    {
        _context.FanficComments.Remove(comment);
        return await _context.SaveChangesAsync() > 0;
    }
}