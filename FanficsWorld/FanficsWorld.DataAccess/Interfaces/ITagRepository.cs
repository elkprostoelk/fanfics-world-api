using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface ITagRepository
{
    Task<ICollection<Tag>> GetAllAsync();
    Task<ICollection<Tag>> GetTop10Async();
}