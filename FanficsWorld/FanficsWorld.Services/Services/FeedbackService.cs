using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Ganss.Xss;

namespace FanficsWorld.Services.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _repository;
    private readonly IHtmlSanitizer _sanitizer;

    public FeedbackService(IFeedbackRepository repository,
        IHtmlSanitizer sanitizer)
    {
        _repository = repository;
        _sanitizer = sanitizer;
    }

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