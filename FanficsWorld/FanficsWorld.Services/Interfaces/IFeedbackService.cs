using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFeedbackService
{
    Task<ServiceResultDto> SendFeedbackAsync(SendFeedbackDto request);
}