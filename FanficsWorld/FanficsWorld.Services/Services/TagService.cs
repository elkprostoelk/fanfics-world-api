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
        try
        {
            var tags = await _repository.GetAllAsync();
            return _mapper.Map<ICollection<TagDto>>(tags);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return null;
        }
    }

    public async Task<ICollection<TagDto>?> GetTop10Async()
    {
        try
        {
            var tags = await _repository.GetTop10Async();
            return _mapper.Map<ICollection<TagDto>>(tags);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return null;
        }
    }

    public async Task<bool> ContainsAllAsync(ICollection<long> ids, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.ContainsAllAsync(ids, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return false;
        }
    }
}