using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;

namespace FanficsWorld.DataAccess.Repositories;

public class FeedbackRepository(FanficsDbContext context) : IFeedbackRepository
{
    private readonly FanficsDbContext _context = context;

    public async Task<bool> SendAsync(Feedback feedback)
    {
        await _context.Feedbacks.AddAsync(feedback);
        return await _context.SaveChangesAsync() > 0;
    }
}