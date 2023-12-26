using FanficsWorld.DataAccess;
using FanficsWorld.Services.Interfaces;

namespace FanficsWorld.Services.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly FanficsDbContext _context;

    public UnitOfWork(FanficsDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}