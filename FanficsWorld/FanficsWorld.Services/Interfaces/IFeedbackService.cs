using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFeedbackService
{
    Task<bool> SendFeedbackAsync(SendFeedbackDto request);
}