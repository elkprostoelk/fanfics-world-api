namespace FanficsWorld.Services.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}