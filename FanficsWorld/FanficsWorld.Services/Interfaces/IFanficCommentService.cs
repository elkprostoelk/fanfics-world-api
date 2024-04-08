using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFanficCommentService
{
    Task<List<FanficCommentDto>> GetFanficCommentsAsync(long fanficId, string? userId);
    Task<ServiceResultDto> SendFanficCommentAsync(SentFanficCommentDto sentFanficCommentDto, string? userId);
    Task<FanficCommentDto?> GetByIdAsync(long commentId);
    Task<ServiceResultDto> SetFanficCommentReactionAsync(long commentId, bool? userLiked, string? userId);
}