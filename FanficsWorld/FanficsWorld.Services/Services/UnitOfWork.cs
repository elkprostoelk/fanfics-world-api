using FanficsWorld.DataAccess;
using FanficsWorld.Services.Interfaces;

namespace FanficsWorld.Services.Services;

public class UnitOfWork(FanficsDbContext context) : IUnitOfWork
{
    private readonly FanficsDbContext _context = context;

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}