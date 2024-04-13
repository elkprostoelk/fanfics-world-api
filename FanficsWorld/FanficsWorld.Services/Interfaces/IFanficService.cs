using FanficsWorld.Common.DTO;
using FanficsWorld.Common.Enums;

namespace FanficsWorld.Services.Interfaces;

public interface IFanficService
{
    Task<FanficDto?> GetByIdAsync(long id);
    Task<FanficPageDto?> GetDisplayFanficByIdAsync(long id);
    Task<ServiceResultDto<long?>> CreateAsync(NewFanficDto newFanficDto, string? userId);
    Task<ServiceResultDto> DeleteAsync(long fanficId);
    Task<ulong?> IncrementFanficViewsCounterAsync(long fanficId);
    Task<ServicePagedResultDto<SimpleFanficDto>> GetPageWithFanficsAsync(int page, int itemsPerPage);
    Task<ServicePagedResultDto<SimpleFanficDto>> SearchFanficsAsync(SearchFanficsDto searchFanficsDto);
    Task SetFanficStatusAsync(long id, FanficStatus fanficStatus);
    Task<ServiceResultDto> EditAsync(EditFanficDto editFanficDto);
}