using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.Common.Enums;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Ganss.XSS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.Services.Services;

public class FanficService : IFanficService
{
    private readonly IFanficRepository _repository;
    private readonly ILogger<FanficService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IHtmlSanitizer _sanitizer;
    private readonly IFandomRepository _fandomRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IConfiguration _configuration;

    public FanficService(
        IFanficRepository repository,
        ILogger<FanficService> logger,
        IMapper mapper,
        IUserRepository userRepository,
        IHtmlSanitizer sanitizer,
        IFandomRepository fandomRepository,
        ITagRepository tagRepository,
        IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
        _sanitizer = sanitizer;
        _fandomRepository = fandomRepository;
        _tagRepository = tagRepository;
        _configuration = configuration;
    }

    public async Task<FanficDto?> GetByIdAsync(long id)
    {
        try
        {
            var fanfic = await _repository.GetAsync(id);
            return _mapper.Map<FanficDto>(fanfic);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return null;
        }
    }

    public async Task<bool> CreateAsync(NewFanficDto newFanficDto, string userId)
    {
        var fanfic = new Fanfic
        {
            Title = _sanitizer.Sanitize(newFanficDto.Title),
            Annotation = _sanitizer.Sanitize(newFanficDto.Annotation),
            Text = _sanitizer.Sanitize(newFanficDto.Text),
            Direction = newFanficDto.Direction,
            Origin = newFanficDto.Origin,
            Rating = newFanficDto.Rating,
            Status = FanficStatus.InProgress,
            AuthorId = userId,
            Coauthors = await _userRepository.GetRangeAsync(newFanficDto.CoauthorIds ?? new List<string>()),
            Fandoms = await _fandomRepository.GetRangeAsync(newFanficDto.FandomIds ?? new List<long>())
        };
        try
        {
            return await _repository.AddAsync(fanfic);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var fanfic = await _repository.GetAsync(id);
            return fanfic is not null && await _repository.DeleteAsync(fanfic);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return false;
        }
    }

    public async Task<bool> AddTagsToFanficAsync(long fanficId, AddTagsDto addTagsDto)
    {
        try
        {
            var fanfic = await _repository.GetAsync(fanficId);

            if (fanfic is null)
            {
                return false;
            }
            var tags = await _tagRepository.GetRangeAsync(addTagsDto.TagIds);
            foreach (var tag in tags)
            {
                fanfic.Tags.Add(tag);
            }

            return await _repository.UpdateAsync(fanfic);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return false;
        }
    }

    public async Task UpdateFanficsStatusesAsync()
    {
        var valueParsed = int.TryParse(_configuration["FanficFrozenAfterDays"], out var fanficFreezingDays);
        if (!valueParsed)
        {
            fanficFreezingDays = 180;
        }
        try
        {
            await foreach (var fanfics in _repository.GetAllInProgressAsync())
            {
                var changedFanfics = new List<Fanfic>();
                for (var i = 0; i < fanfics.Count(); i++)
                {
                    var current = fanfics.ElementAt(i);
                    var difference = (DateTime.Now - current.LastModified.GetValueOrDefault()).Days;
                    if (difference >= fanficFreezingDays)
                    {
                        current.Status = FanficStatus.Frozen;
                        changedFanfics.Add(current);
                    }
                }

                await _repository.UpdateRangeAsync(changedFanfics);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
        }
    }
}