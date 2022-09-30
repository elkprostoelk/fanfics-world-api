using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IFanficRepository
{
    Task<Fanfic?> GetAsync(long id);
    Task<bool> AddAsync(Fanfic fanfic);
}