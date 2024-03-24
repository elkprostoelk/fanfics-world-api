﻿using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.Common.Enums;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;

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
}