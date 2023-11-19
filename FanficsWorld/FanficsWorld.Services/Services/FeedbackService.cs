using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Ganss.Xss;

namespace FanficsWorld.Services.Services;

public class FeedbackService(
    IFeedbackRepository repository,
    IHtmlSanitizer sanitizer
        ) : IFeedbackService
{
    private readonly IFeedbackRepository _repository = repository;
    private readonly IHtmlSanitizer _sanitizer = sanitizer;

    public async Task<bool> SendFeedbackAsync(SendFeedbackDto request)
    {
        var feedback = new Feedback
        {
            Name = request.Name is not null ? _sanitizer.Sanitize(request.Name) : null,
            Text = _sanitizer.Sanitize(request.Text),
            Email = request.Email,
            Reviewed = false
        };
        return await _repository.SendAsync(feedback);
    }
}