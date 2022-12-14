using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFandomService
{
    Task<ICollection<SimpleFandomDto>> GetTop10FandomsAsync();
    Task<long?> CreateAsync(NewFandomDto newFandomDto);
    Task<FandomDto?> GetFandomWithFanficsAsync(long id);
}