using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Ganss.Xss;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.Services.Services;

public class FanficCommentService : IFanficCommentService
{
    private readonly IFanficCommentRepository _repository;
    private readonly IFanficRepository _fanficRepository;
    private readonly IMapper _mapper;
    private readonly IHtmlSanitizer _htmlSanitizer;
    private readonly ILogger<FanficCommentService> _logger;

    public FanficCommentService(
        IFanficCommentRepository repository,
        IFanficRepository fanficRepository,
        IMapper mapper,
        IHtmlSanitizer htmlSanitizer,
        ILogger<FanficCommentService> logger)
    {
        _repository = repository;
        _fanficRepository = fanficRepository;
        _mapper = mapper;
        _htmlSanitizer = htmlSanitizer;
        _logger = logger;
    }

    public async Task<List<FanficCommentDto>> GetFanficCommentsAsync(long fanficId, string? userId)
    {
        var fanfic = await _fanficRepository.GetAsync(fanficId);
        if (fanfic is null)
        {
            return [];
        }

        var fanficComments = await _repository.GetCommentsAsync(fanficId);
        return fanficComments.Select(fc => new FanficCommentDto
        {
            Id = fc.Id,
            CreatedDate = fc.CreatedDate,
            Text = fc.Text,
            Author = _mapper.Map<SimpleUserDto>(fc.Author),
            CurrentUserReaction = !string.IsNullOrEmpty(userId)
                ? fc.Reactions.FirstOrDefault(r => r.UserId == userId)?.IsLike
                : null,
            LikesCount = fc.Reactions.Count(r => r.IsLike),
            DislikesCount = fc.Reactions.Count(r => !r.IsLike)
        }).ToList();
    }

    public async Task<ServiceResultDto> SendFanficCommentAsync(SentFanficCommentDto sentFanficCommentDto, string? userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Could not recognize a user ID. Cannot send a comment anonymously.");
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = "Comment author is not specified!"
            };
        }

        var fanfic = await _fanficRepository.GetAsync(sentFanficCommentDto.FanficId);
        if (fanfic is null)
        {
            _logger.LogWarning("Could not find a fanfic {FanficId}. Cannot send a comment for a non-existing fanfic.", sentFanficCommentDto.FanficId);
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = "The fanfic does not exist!"
            };
        }

        var comment = new FanficComment
        {
            AuthorId = userId,
            CreatedDate = DateTime.Now,
            FanficId = sentFanficCommentDto.FanficId,
            Text = _htmlSanitizer.Sanitize(sentFanficCommentDto.Comment)
        };

        var commentAdded = await _repository.AddCommentAsync(comment);
        return commentAdded
            ? new ServiceResultDto()
            : new ServiceResultDto { IsSuccess = false, ErrorMessage = "Cannot add a comment!" };
    }

    public async Task<FanficCommentDto?> GetByIdAsync(long commentId)
    {
        var comment = await _repository.GetAsync(commentId);
        return _mapper.Map<FanficCommentDto?>(comment);
    }

    public async Task<ServiceResultDto> SetFanficCommentReactionAsync(long commentId, bool? userLiked, string? userId)
    {
        var comment = await _repository.GetAsync(commentId, asNoTracking: false);
        if (comment is null)
        {
            _logger.LogWarning("Could not find a fanfic comment {CommentId}.", commentId);
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = $"Fanfic comment {commentId} has not been found!"
            };
        }

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Could not recognize a user.");
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = "User has not been recognized!"
            };
        }

        var existingFanficCommentReaction = comment.Reactions?.FirstOrDefault(r => r.UserId == userId);

        if (IsReactionAlreadySet(existingFanficCommentReaction, userLiked))
        {
            _logger.LogWarning("A reaction {UserLiked} for a comment {CommentId} is already set.", userLiked, commentId);
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = "This reaction has already been set!"
            };
        }

        if (!userLiked.HasValue)
        {
            if (existingFanficCommentReaction is null)
            {
                _logger.LogWarning("Cannot unset the reaction for a comment {CommentId} because it does not exist.", commentId);
                return new ServiceResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "This reaction cannot be unset!"
                };
            }

            var deleted = await _repository.RemoveCommentReactionAsync(existingFanficCommentReaction);

            if (!deleted)
            {
                return new ServiceResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Cannot remove this reaction!"
                };
            }
        }
        else
        {
            if (existingFanficCommentReaction is null)
            {
                var reaction = new FanficCommentReaction
                {
                    FanficCommentId = commentId,
                    UserId = userId,
                    IsLike = userLiked.Value
                };
                var added = await _repository.AddCommentReactionAsync(reaction);
                if (!added)
                {
                    return new ServiceResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cannot add this reaction!"
                    };
                }
            }
            else
            {
                existingFanficCommentReaction.IsLike = userLiked.Value;
                var changedReaction = await _repository.UpdateCommentReactionAsync(existingFanficCommentReaction);
                if (!changedReaction)
                {
                    return new ServiceResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cannot change this reaction!"
                    };
                }
            }
        }
        
        return new ServiceResultDto();
    }

    private static bool IsReactionAlreadySet(FanficCommentReaction? existingFanficCommentReaction, bool? userLiked)
    {
        bool? existingFanficCommentReactionIsLike = existingFanficCommentReaction?.IsLike;
        return existingFanficCommentReactionIsLike == userLiked;
    }
}