using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.Services.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public TagService(ITagRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TagDto>> GetAllAsync(string? title = null)
    {
        var tagsQuery = ApplyNameFilter(title);

        return await tagsQuery
            .Select(tag => _mapper.Map<TagDto>(tag))
            .ToListAsync();
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
        return _mapper.Map<TagWithFanficsDto>(tag);
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
                FanficsCount = t.Fanfics.Count
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
}