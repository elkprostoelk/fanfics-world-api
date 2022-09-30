using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IFanficService
{
    Task<FanficDTO?> GetByIdAsync(long id);
    Task<bool> CreateAsync(NewFanficDTO newFanficDto);
}