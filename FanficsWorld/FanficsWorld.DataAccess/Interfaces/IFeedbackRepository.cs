using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IFeedbackRepository
{
    Task<bool> SendAsync(Feedback feedback);
}