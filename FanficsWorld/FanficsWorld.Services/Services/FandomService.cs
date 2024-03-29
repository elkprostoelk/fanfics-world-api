﻿using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.Services.Services;

public class FandomService : IFandomService
{
    private readonly IFandomRepository _repository;
    private readonly IMapper _mapper;

    public FandomService(IFandomRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<SimpleFandomDto>> GetTop10FandomsAsync()
    {
        var fandoms = await _repository.GetTop10Async();
        return _mapper.Map<List<SimpleFandomDto>>(fandoms);
    }

    public async Task<List<SimpleFandomDto>> SearchByTitleAsync(string title) =>
        await _repository.GetFandoms(title)
            .Select(fdom => _mapper.Map<SimpleFandomDto>(fdom))
            .ToListAsync();

    public async Task<long?> CreateAsync(NewFandomDto newFandomDto)
    {
        var fandom = _mapper.Map<Fandom>(newFandomDto);
        return await _repository.CreateAsync(fandom);
    }

    public async Task<FandomDto?> GetFandomWithFanficsAsync(long id)
    {
        var fandom = await _repository.GetAsync(id);
        return _mapper.Map<FandomDto?>(fandom);
    }
}