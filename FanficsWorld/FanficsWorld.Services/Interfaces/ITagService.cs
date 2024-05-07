using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface ITagService
{
    Task<List<TagDto>> GetAllAsync(string? title = null);
    Task<List<TagDto>> GetTop10Async();
    Task<bool> ContainsAllAsync(List<long> ids, CancellationToken cancellationToken);
    Task<TagWithFanficsDto?> GetFullByIdAsync(long id);
    Task<ServicePagedResultDto<AdminPageTagDto>> GetAllAsync(string? searchByName, int page, int itemsPerPage);
    Task<ServiceResultDto> CreateAsync(NewTagDto newTagDto);
    Task<ServiceResultDto> DeleteAsync(long id);
}