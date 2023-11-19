using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.Services.Services;

public class TagService(ITagRepository repository,
    IMapper mapper) : ITagService
{
    private readonly ITagRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ICollection<TagDto>?> GetAllAsync(string? title = null)
    {
        var tagsReq = _repository.GetAll();
        if (!string.IsNullOrWhiteSpace(title))
        {
            tagsReq = tagsReq.Where(tag => tag.Name.Contains(title));
        }

        return await tagsReq.Select(tag => _mapper.Map<TagDto>(tag)).ToListAsync();
    }

    public async Task<ICollection<TagDto>?> GetTop10Async()
    {
        var tags = await _repository.GetTop10Async();
        return _mapper.Map<ICollection<TagDto>>(tags);
    }

    public async Task<bool> ContainsAllAsync(ICollection<long> ids,
        CancellationToken cancellationToken) =>
        await _repository.ContainsAllAsync(ids, cancellationToken);

    public async Task<TagWithFanficsDto?> GetFullByIdAsync(long id)
    {
        var tag = await _repository.GetAsync(id);
        return _mapper.Map<TagWithFanficsDto>(tag);
    }
}