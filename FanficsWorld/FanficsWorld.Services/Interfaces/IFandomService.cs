using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFandomService
{
    Task<List<SimpleFandomDto>> GetTop10FandomsAsync();
    Task<List<SimpleFandomDto>> SearchByTitleAsync(string title);
    Task<long?> CreateAsync(NewFandomDto newFandomDto);
    Task<FandomDto?> GetFandomWithFanficsAsync(long id);
}