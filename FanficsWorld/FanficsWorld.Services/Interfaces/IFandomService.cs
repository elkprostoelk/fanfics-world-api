using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFandomService
{
    Task<ICollection<SimpleFandomDTO>> GetTop10FandomsAsync();
    Task<bool> CreateAsync(NewFandomDto newFandomDto);
}