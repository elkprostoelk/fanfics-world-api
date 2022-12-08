using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Ganss.XSS;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.Services.Services;

public class FeedbackService : IFeedbackService
{
    private readonly ILogger<FeedbackService> _logger;
    private readonly IFeedbackRepository _repository;
    private readonly IHtmlSanitizer _sanitizer;

    public FeedbackService(
        ILogger<FeedbackService> logger,
        IFeedbackRepository repository,
        IHtmlSanitizer sanitizer
        )
    {
        _logger = logger;
        _repository = repository;
        _sanitizer = sanitizer;
    }
    
    public async Task<bool> SendFeedbackAsync(SendFeedbackDto request)
    {
        try
        {
            var feedback = new Feedback
            {
                Name = _sanitizer.Sanitize(request.Name),
                Text = _sanitizer.Sanitize(request.Text),
                Email = request.Email,
                Reviewed = false
            };
            return await _repository.SendAsync(feedback);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return false;
        }
    }
}