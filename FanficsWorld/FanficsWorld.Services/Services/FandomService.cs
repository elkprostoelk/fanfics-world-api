using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.Services.Services;

public class FandomService : IFandomService
{
    private readonly IFandomRepository _repository;
    private readonly ILogger<FandomService> _logger;
    private readonly IMapper _mapper;

    public FandomService(
        IFandomRepository repository,
        ILogger<FandomService> logger,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ICollection<SimpleFandomDTO>> GetTop10FandomsAsync()
    {
        try
        {
            var fandoms = await _repository.GetTop10Async();
            return _mapper.Map<ICollection<SimpleFandomDTO>>(fandoms);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return new List<SimpleFandomDTO>();
        }
    }

    public async Task<bool> CreateAsync(NewFandomDto newFandomDto)
    {
        try
        {
            var fandom = _mapper.Map<Fandom>(newFandomDto);
            return await _repository.CreateAsync(fandom);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return false;
        }
    }
}