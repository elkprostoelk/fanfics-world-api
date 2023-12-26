using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;

namespace FanficsWorld.DataAccess.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly FanficsDbContext _context;

    public FeedbackRepository(FanficsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SendAsync(Feedback feedback)
    {
        await _context.Feedbacks.AddAsync(feedback);
        return await _context.SaveChangesAsync() > 0;
    }
}