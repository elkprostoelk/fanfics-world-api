using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.Services.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly ILogger<TagService> _logger;
    private readonly IMapper _mapper;

    public TagService(
        ITagRepository repository,
        ILogger<TagService> logger,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<TagDto>> GetAllAsync(string? title = null)
    {
        var tagsQuery = ApplyNameFilter(title);

        return await tagsQuery
            .Where(t => !t.IsDeleted)
            .Select(tag => _mapper.Map<TagDto>(tag))
            .ToListAsync();
    }

    public async Task<List<TagDto>> GetTop10Async()
    {
        var tags = await _repository.GetTop10Async();
        return _mapper.Map<List<TagDto>>(tags);
    }

    public async Task<bool> ContainsAllAsync(List<long> ids,
        CancellationToken cancellationToken) =>
        await _repository.ContainsAllAsync(ids, cancellationToken);

    public async Task<TagWithFanficsDto?> GetFullByIdAsync(long id)
    {
        var tag = await _repository.GetAsync(id);

        return tag is { IsDeleted: true }
            ? default
            : _mapper.Map<TagWithFanficsDto>(tag);
    }

    public async Task<ServicePagedResultDto<AdminPageTagDto>> GetAllAsync(string? searchByName, int page, int itemsPerPage)
    {
        var tagsQuery = ApplyNameFilter(searchByName);
        var totalItemsCount = await tagsQuery.CountAsync();
        var tags = await tagsQuery
            .Select(t => new AdminPageTagDto
            {
                Id = t.Id,
                Name = t.Name,
                FanficsCount = t.Fanfics.Count,
                IsDeleted = t.IsDeleted
            }).ToListAsync();

        return new ServicePagedResultDto<AdminPageTagDto>
        {
            CurrentPage = page,
            ItemsPerPage = itemsPerPage,
            PageContent = tags,
            TotalItems = totalItemsCount,
            PagesCount = Convert.ToInt32(Math.Ceiling(totalItemsCount / (float)itemsPerPage))
        };
    }

    public async Task<ServiceResultDto> CreateAsync(NewTagDto newTagDto)
    {
        var tag = _mapper.Map<Tag>(newTagDto);
        var created = await _repository.AddAsync(tag);

        return new ServiceResultDto
        {
            IsSuccess = created,
            ErrorMessage = !created ? "Failed to create a tag!" : null
        };
    }

    public async Task<ServiceResultDto> DeleteAsync(long id)
    {
        var tag = await _repository.GetAsync(id);
        if (tag is null)
        {
            _logger.LogWarning("Failed to delete a tag {Id}. It does not exist.", id);
            
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = "The tag does not exist!"
            };
        }

        var deleted = await _repository.DeleteAsync(tag);
        return new ServiceResultDto
        {
            IsSuccess = deleted,
            ErrorMessage = !deleted ? "Failed to delete a tag!" : null
        };
    }
    
    private IQueryable<Tag> ApplyNameFilter(string? name)
    {
        var tagsQuery = _repository.GetAll();
        if (!string.IsNullOrWhiteSpace(name))
        {
            tagsQuery = tagsQuery.Where(tag => tag.Name.Contains(name));
        }

        return tagsQuery;
    }
}