using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.Common.Enums;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Ganss.XSS;
using Microsoft.EntityFrameworkCore;
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
    private readonly IUnitOfWork _unitOfWork;

    public FanficService(
        IFanficRepository repository,
        ILogger<FanficService> logger,
        IMapper mapper,
        IUserRepository userRepository,
        IHtmlSanitizer sanitizer,
        IFandomRepository fandomRepository,
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
        _sanitizer = sanitizer;
        _fandomRepository = fandomRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
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

    public async Task<long?> CreateAsync(NewFanficDto newFanficDto, string userId)
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
            AuthorId = userId
        };
        try
        {
            var fanficId = await _repository.AddAsync(fanfic);

            if (fanficId.HasValue)
            {
                var coauthors = await _userRepository.GetRangeAsync(newFanficDto.CoauthorIds ?? new List<string>());
                var fandoms = await _fandomRepository.GetRangeAsync(newFanficDto.FandomIds ?? new List<long>());
                var tags = await _tagRepository.GetRangeAsync(newFanficDto.TagIds ?? new List<long>());
                var coauthorsNotEmpty = coauthors.Any();
                var fandomsNotEmpty = fandoms.Any();
                var tagsNotEmpty = tags.Any();
                if (coauthorsNotEmpty)
                {
                    coauthors.ToList().ForEach(author => fanfic.Coauthors.Add(author));
                }
                if (fandomsNotEmpty)
                {
                    fandoms.ToList().ForEach(fandom => fanfic.Fandoms.Add(fandom));
                }
                if (tagsNotEmpty)
                {
                    tags.ToList().ForEach(tag => fanfic.Tags.Add(tag));
                }

                if (coauthorsNotEmpty || fandomsNotEmpty || tagsNotEmpty)
                {
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            
            return fanficId;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return null;
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

    public async Task<ulong?> IncrementFanficViewsCounterAsync(long fanficId)
    {
        try
        {
            var fanfic = await _repository.GetAsync(fanficId);
            if (fanfic is null)
            {
                return null;
            }
            
            var newCount = ++fanfic.Views;
            var updated = await _repository.UpdateAsync(fanfic);
            return updated ? newCount : null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return null;
        }
    }

    public async Task<ServicePagedResultDto<SimpleFanficDto>> GetPageWithFanficsAsync(int page, int itemsPerPage)
    {
        try
        {
            var fanfics = await _repository.GetAllPagedAsync(page, itemsPerPage)
                .ToListAsync();
            var fanficDtos = _mapper.Map<ICollection<SimpleFanficDto>>(fanfics);
            var totalItems = await _repository.CountAsync();
            var pagesCount = Convert.ToInt32(Math.Ceiling(totalItems / (double)itemsPerPage));
            return new ServicePagedResultDto<SimpleFanficDto>
            {
                PageContent = fanficDtos,
                CurrentPage = page,
                TotalItems = totalItems,
                PagesCount = pagesCount,
                ItemsPerPage = itemsPerPage
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return new ServicePagedResultDto<SimpleFanficDto>
            {
                PageContent = new List<SimpleFanficDto>()
            };
        }
    }

    public async Task<long> CountInProgressAsync() =>
        await _repository.CountByStatusAsync(FanficStatus.InProgress);
    
    public async Task<ICollection<MinifiedFanficDto>> GetMinifiedInProgressAsync(int chunkSize)
    {
        try
        {
            var fanfics = await _repository.GetAllInProgress(chunkSize).ToListAsync();
            return _mapper.Map<List<MinifiedFanficDto>>(fanfics);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return new List<MinifiedFanficDto>();
        }
    }

    public async Task SetFanficStatusAsync(long id, FanficStatus fanficStatus)
    {
        try
        {
            var fanfic = await _repository.GetAsync(id);
            if (fanfic is not null)
            {
                fanfic.Status = fanficStatus;
                await _repository.UpdateAsync(fanfic);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
        }
    }
}