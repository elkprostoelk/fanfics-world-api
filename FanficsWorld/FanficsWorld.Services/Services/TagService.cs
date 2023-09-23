using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.Services.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TagService> _logger;

    public TagService(ITagRepository repository,
        IMapper mapper,
        ILogger<TagService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<TagDto>?> GetAllAsync()
    {
        var tags = await _repository.GetAllAsync();
        return _mapper.Map<ICollection<TagDto>>(tags);
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