using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserRepository> _logger;
    private readonly IMemoryCache _cache;

    public UserRepository(UserManager<User> userManager,
        ILogger<UserRepository> logger,
        IMemoryCache cache)
    {
        _userManager = userManager;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IdentityResult> RegisterUserAsync(User user, string password, string role)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            var roleAddingResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleAddingResult.Succeeded)
            {
                _logger.LogError("Error(s) occured while adding user {UserId} to role {Role}: {ErrorsList}", 
                    user.Id, role, string.Join(", ", result.Errors.Select(err => $"{err.Code}: {err.Description}")));
            }
        }
        
        return result;
    }

    public async Task<User?> GetAsync(string idOrUserName)
    {
        if (Guid.TryParse(idOrUserName, out var id))
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        return await _userManager.FindByNameAsync(idOrUserName);
    }

    public async Task<ICollection<User>> GetRangeAsync(ICollection<string> coauthorIds) =>
        await _userManager.Users.Where(u => coauthorIds.Contains(u.Id)).ToListAsync();

    public async Task<ICollection<User>> GetChunkAsync(string? userName, int chunkNumber, int chunkSize)
    {
        var cacheKey = $"users_chunk_no_{chunkNumber}_size_{chunkSize}";
        var isSearchByName = !string.IsNullOrWhiteSpace(userName);
        if (isSearchByName)
        {
            cacheKey = string.Concat(cacheKey, $"_username_{userName}");
        }
        
        var isListCached = _cache.TryGetValue(cacheKey, out List<User>? users);
        if (!isListCached)
        {
            IQueryable<User> usersQuery = _userManager.Users.AsNoTracking()
                .OrderBy(u => u.UserName)
                .Skip(chunkNumber * chunkSize)
                .Take(chunkSize);
            
            if (isSearchByName)
            {
                usersQuery = usersQuery.Where(u =>
                    u.UserName!.Contains(userName!));
            }
            users = await usersQuery
                .ToListAsync();
            
            _cache.Set(cacheKey, users, TimeSpan.FromHours(1));
        }
        
        return users ?? [];
    }

    public async Task<long> CountAsync(string? currentUserId = null) =>
        await _userManager.Users
            .LongCountAsync(u => u.Id != currentUserId);
}