using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.Common.Enums;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace FanficsWorld.Services.Services;

public class FanficService : IFanficService
{
    private readonly IFanficRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IHtmlSanitizer _sanitizer;
    private readonly IFandomRepository _fandomRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FanficService(IFanficRepository repository,
        IMapper mapper,
        IUserRepository userRepository,
        IHtmlSanitizer sanitizer,
        IFandomRepository fandomRepository,
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
        _sanitizer = sanitizer;
        _fandomRepository = fandomRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FanficDto?> GetByIdAsync(long id)
    {
        var fanfic = await _repository.GetAsync(id);
        return _mapper.Map<FanficDto?>(fanfic);
    }

    public async Task<FanficPageDto?> GetDisplayFanficByIdAsync(long id)
    {
        var fanfic = await _repository.GetAsync(id);
        return _mapper.Map<FanficPageDto>(fanfic);
    }

    public async Task<long?> CreateAsync(NewFanficDto newFanficDto, string userId)
    {
        var fanfic = new Fanfic
        {
            Title = _sanitizer.Sanitize(newFanficDto.Title),
            Annotation = newFanficDto.Annotation is not null ? _sanitizer.Sanitize(newFanficDto.Annotation) : null,
            Text = _sanitizer.Sanitize(newFanficDto.Text),
            Direction = newFanficDto.Direction,
            Origin = newFanficDto.Origin,
            Rating = newFanficDto.Rating,
            Status = FanficStatus.InProgress,
            AuthorId = userId,
            Coauthors = await _userRepository.GetRangeAsync(newFanficDto.CoauthorIds ?? []),
            Fandoms = await _fandomRepository.GetRangeAsync(newFanficDto.FandomIds ?? []),
            Tags = await _tagRepository.GetRangeAsync(newFanficDto.TagIds ?? [])
        };
        
        
        
        var fanficId = await _repository.AddAsync(fanfic);
        return fanficId;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var fanfic = await _repository.GetAsync(id);
        return fanfic is not null && await _repository.DeleteAsync(fanfic);
    }

    public async Task<bool> AddTagsToFanficAsync(long fanficId, AddTagsDto addTagsDto)
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

    public async Task<ulong?> IncrementFanficViewsCounterAsync(long fanficId)
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

    public async Task<ServicePagedResultDto<SimpleFanficDto>> GetPageWithFanficsAsync(int page, int itemsPerPage)
    {
        var fanfics = await _repository.GetAllPaged(page, itemsPerPage)
            .ToListAsync();
        var fanficDtos = _mapper.Map<List<SimpleFanficDto>>(fanfics);
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

    public async Task SetFanficStatusAsync(long id, FanficStatus fanficStatus)
    {
        var fanfic = await _repository.GetAsync(id);
        if (fanfic is not null)
        {
            fanfic.Status = fanficStatus;
            await _repository.UpdateAsync(fanfic);
        }
    }

    public async Task<ServicePagedResultDto<SimpleFanficDto>> SearchFanficsAsync(SearchFanficsDto searchFanficsDto)
    {
        const int defaultPage = 1;
        const int defaultPageSize = 20;

        if (searchFanficsDto is null)
        {
            return await GetPageWithFanficsAsync(defaultPage, defaultPageSize);
        }

        IQueryable<Fanfic> fanficsQuery = _repository.GetAll();

        fanficsQuery = FilterFanfics(fanficsQuery, searchFanficsDto);
        fanficsQuery = ApplySorting(fanficsQuery, searchFanficsDto.SortBy, searchFanficsDto.SortOrder);

        var totalItemsCount = await fanficsQuery.CountAsync();
        fanficsQuery = ApplyPaging(fanficsQuery, searchFanficsDto);

        var pageList = await fanficsQuery.ToListAsync();
        var dtoList = _mapper.Map<List<SimpleFanficDto>>(pageList);
        var pagesCount = Convert.ToInt32(Math.Ceiling(totalItemsCount / (double)searchFanficsDto.ItemsPerPage));

        return new ServicePagedResultDto<SimpleFanficDto>
        {
            PageContent = dtoList,
            TotalItems = totalItemsCount,
            CurrentPage = searchFanficsDto.Page,
            ItemsPerPage = searchFanficsDto.ItemsPerPage,
            PagesCount = pagesCount
        };
    }

    private static IQueryable<Fanfic> ApplyPaging(IQueryable<Fanfic> fanficsQuery, SearchFanficsDto searchFanficsDto)
    {
        return fanficsQuery
            .Skip((searchFanficsDto.Page - 1) * searchFanficsDto.ItemsPerPage)
            .Take(searchFanficsDto.ItemsPerPage);
    }

    private static IQueryable<Fanfic> ApplySorting(IQueryable<Fanfic> fanficsQuery, SortingValue? sortBy, SortingOrder? sortingOrder)
    {
        switch (sortBy)
        {
            case SortingValue.CreationDate:
            case null:
                fanficsQuery = sortingOrder == SortingOrder.Ascending
                    ? fanficsQuery.OrderBy(f => f.CreatedDate)
                    : fanficsQuery.OrderByDescending(f => f.CreatedDate);
                break;
            case SortingValue.Title:
                fanficsQuery = sortingOrder == SortingOrder.Ascending
                    ? fanficsQuery.OrderBy(f => f.Title)
                    : fanficsQuery.OrderByDescending(f => f.Title);
                break;
        }

        return fanficsQuery;
    }

    private static IQueryable<Fanfic> FilterFanfics(IQueryable<Fanfic> query, SearchFanficsDto searchFanficsDto)
    {
        if (!string.IsNullOrWhiteSpace(searchFanficsDto.SearchByTitle))
        {
            query = query.Where(f => f.Title.Contains(searchFanficsDto.SearchByTitle));
        }

        if (searchFanficsDto.FandomIds?.Count > 0)
        {
            query = query.Where(f => f.Fandoms.Any(fandom => searchFanficsDto.FandomIds.Contains(fandom.Id)));
        }

        if (searchFanficsDto.TagIds?.Count > 0)
        {
            query = query.Where(f => f.Tags.Any(tag => searchFanficsDto.TagIds.Contains(tag.Id)));
        }

        query = ApplyOriginsFilter(query, searchFanficsDto.Origins);
        query = ApplyDirectionsFilter(query, searchFanficsDto.Directions);
        query = ApplyRatingsFilter(query, searchFanficsDto.Ratings);
        query = ApplyStatusesFilter(query, searchFanficsDto.Statuses);

        return query;
    }

    private static IQueryable<Fanfic> ApplyStatusesFilter(IQueryable<Fanfic> query, string? fanficStatuses)
    {
        if (string.IsNullOrEmpty(fanficStatuses))
        {
            return query;
        }

        var directions = fanficStatuses
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select<string, FanficStatus?>(d =>
                {
                    var parsed = Enum.TryParse<FanficStatus>(d, ignoreCase: true, out var status);
                    return status;
                })
                .TakeWhile(d => d.HasValue)
                .ToList();
        return query = query.Where(f => directions.Contains(f.Status));
    }

    private static IQueryable<Fanfic> ApplyRatingsFilter(IQueryable<Fanfic> query, string? fanficRatings)
    {
        if (string.IsNullOrEmpty(fanficRatings))
        {
            return query;
        }

        var directions = fanficRatings
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select<string, FanficRating?>(d =>
                {
                    var parsed = Enum.TryParse<FanficRating>(d, ignoreCase: true, out var rating);
                    return rating;
                })
                .TakeWhile(d => d.HasValue)
                .ToList();
        return query = query.Where(f => directions.Contains(f.Rating));
    }

    private static IQueryable<Fanfic> ApplyDirectionsFilter(IQueryable<Fanfic> query, string? fanficDirections)
    {
        if (string.IsNullOrEmpty(fanficDirections))
        {
            return query;
        }

        var directions = fanficDirections
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select<string, FanficDirection?>(d =>
                {
                    var parsed = Enum.TryParse<FanficDirection>(d, ignoreCase: true, out var direction);
                    return direction;
                })
                .TakeWhile(d => d.HasValue)
                .ToList();
        return query = query.Where(f => directions.Contains(f.Direction));
    }

    private static IQueryable<Fanfic> ApplyOriginsFilter(IQueryable<Fanfic> query, string? fanficOrigins)
    {
        if (string.IsNullOrEmpty(fanficOrigins))
        {
            return query;
        }

        var directions = fanficOrigins
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select<string, FanficOrigin?>(d =>
                {
                    var parsed = Enum.TryParse<FanficOrigin>(d, ignoreCase: true, out var origin);
                    return origin;
                })
                .TakeWhile(d => d.HasValue)
                .ToList();
        return query = query.Where(f => directions.Contains(f.Origin));
    }
}