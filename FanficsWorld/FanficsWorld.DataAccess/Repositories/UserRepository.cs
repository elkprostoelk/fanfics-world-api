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

    public UserRepository(
        UserManager<User> userManager,
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

    public async Task<List<User>> GetRangeAsync(List<string> userIds) =>
        await _userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

    public async Task<List<User>> GetListAsync(string? userName)
    {
        var cacheKey = "simple_users";
        var isSearchByName = !string.IsNullOrWhiteSpace(userName);
        if (isSearchByName)
        {
            cacheKey = string.Concat(cacheKey, $"_userSearch_{userName}");
        }
        
        var isListCached = _cache.TryGetValue(cacheKey, out List<User>? users);
        if (isListCached)
        {
            return users ?? [];
        }

        var usersQuery = _userManager.Users.AsNoTracking();
        
        if (isSearchByName)
        {
            usersQuery = usersQuery.Where(u =>
                u.UserName!.Contains(userName!));
        }
        users = await usersQuery
            .OrderBy(u => u.UserName)
            .ToListAsync();
            
        _cache.Set(cacheKey, users, TimeSpan.FromHours(1));
        return users;
    }

    public async Task<long> CountAsync(string? currentUserId = null)
    {
        var usersQuery = _userManager.Users;
        if (!string.IsNullOrWhiteSpace(currentUserId))
        {
            usersQuery = usersQuery.Where(u => u.Id != currentUserId);
        }
        
        return await usersQuery
            .LongCountAsync();
    }

    public async Task<bool> CheckPasswordAsync(User user, string password) =>
        await _userManager.CheckPasswordAsync(user, password);

    public async Task<bool> UserExistsAsync(string idOrUserName) =>
        await _userManager.Users.AnyAsync(u => u.Id == idOrUserName
                                               || (!string.IsNullOrEmpty(u.UserName)
                                                   && u.UserName.Contains(idOrUserName)));

    public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword) =>
        await _userManager.ChangePasswordAsync(
            user,
            currentPassword,
            newPassword);

    public async Task<List<string>> GetRolesAsync(User user)
    {
        var users = await _userManager.GetRolesAsync(user);
        return users.ToList();
    }
}