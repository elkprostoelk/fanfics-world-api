﻿using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.Services.Services;

public class FanficService : IFanficService
{
    private readonly IFanficRepository _repository;
    private readonly ILogger<FanficService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public FanficService(
        IFanficRepository repository,
        ILogger<FanficService> logger,
        IMapper mapper,
        IUserRepository userRepository)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<FanficDTO?> GetByIdAsync(long id)
    {
        try
        {
            var fanfic = await _repository.GetAsync(id);
            return _mapper.Map<FanficDTO>(fanfic);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while executing the service");
            return null;
        }
    }

    public async Task<bool> CreateAsync(NewFanficDTO newFanficDto, string userId)
    {
        var fanfic = new Fanfic
        {
            Title = newFanficDto.Title,
            Annotation = newFanficDto.Annotation,
            Text = newFanficDto.Text,
            AuthorId = userId,
            Coauthors = newFanficDto.CoauthorIds is not null
                ? await _userRepository.GetRangeAsync(newFanficDto.CoauthorIds)
                : null
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
}