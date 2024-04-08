using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IFanficCommentRepository
{
    Task<List<FanficComment>> GetCommentsAsync(long fanficId);
    Task<bool> AddCommentAsync(FanficComment comment);
    Task<FanficComment?> GetAsync(long commentId, bool asNoTracking = true);
    Task<bool> RemoveCommentReactionAsync(FanficCommentReaction fanficCommentReaction);
    Task<bool> AddCommentReactionAsync(FanficCommentReaction reaction);
    Task<bool> UpdateCommentReactionAsync(FanficCommentReaction fanficCommentReaction);
}