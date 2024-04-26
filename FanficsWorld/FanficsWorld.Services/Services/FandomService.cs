using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

    public async Task<List<SimpleFandomDto>> GetTop10FandomsAsync()
    {
        var fandoms = await _repository.GetTop10Async();
        return _mapper.Map<List<SimpleFandomDto>>(fandoms);
    }

    public async Task<List<SimpleFandomDto>> SearchByTitleAsync(string title) =>
        await _repository.GetFandoms(title)
            .Select(fdom => _mapper.Map<SimpleFandomDto>(fdom))
            .ToListAsync();

    public async Task<ServiceResultDto<long?>> CreateAsync(NewFandomDto newFandomDto)
    {
        var fandom = _mapper.Map<Fandom>(newFandomDto);
        var createdFandomId = await _repository.CreateAsync(fandom);
        return new ServiceResultDto<long?>
        {
            IsSuccess = createdFandomId.HasValue,
            Result = createdFandomId,
            ErrorMessage = createdFandomId.HasValue ? null : "Cannot create a fandom!"
        };
    }

    public async Task<FandomDto?> GetFandomWithFanficsAsync(long id)
    {
        var fandom = await _repository.GetAsync(id);
        return _mapper.Map<FandomDto?>(fandom);
    }

    public async Task<ServicePagedResultDto<AdminPageFandomDto>> GetForAdminPageAsync(SearchFandomsDto searchFandomsDto)
    {
        var fandomsQuery = _repository.GetAll();
        if (!string.IsNullOrEmpty(searchFandomsDto.SearchByName))
        {
            fandomsQuery = fandomsQuery.Where(f => f.Title.Contains(searchFandomsDto.SearchByName));
        }

        var fandomsCount = await fandomsQuery.CountAsync();
        var fandoms = await fandomsQuery
            .Skip((searchFandomsDto.Page - 1) * searchFandomsDto.ItemsPerPage)
            .Take(searchFandomsDto.ItemsPerPage)
            .Select(f => new AdminPageFandomDto
            {
                Id = f.Id,
                Title = f.Title,
                FanficsCount = f.Fanfics.Count,
                IsDeleted = f.IsDeleted
            })
            .ToListAsync();

        return new ServicePagedResultDto<AdminPageFandomDto>
        {
            PageContent = fandoms,
            TotalItems = fandomsCount,
            CurrentPage = searchFandomsDto.Page,
            PagesCount = Convert.ToInt32(Math.Ceiling(fandomsCount / (float)searchFandomsDto.ItemsPerPage)),
            ItemsPerPage = searchFandomsDto.ItemsPerPage
        };
    }

    public async Task<ServiceResultDto> DeleteAsync(long id)
    {
        var fandom = await _repository.GetAsync(id);
        if (fandom is null)
        {
            _logger.LogWarning("Cannot delete the fandom {Id}. It does not exist.", id);
            
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = "The fandom does not exist!"
            };
        }

        var deleted = await _repository.DeleteAsync(fandom);
        return new ServiceResultDto
        {
            IsSuccess = deleted,
            ErrorMessage = "Failed to delete this fanfic!"
        };
    }
}