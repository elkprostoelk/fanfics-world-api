using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FanficsWorld.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        UserManager<User> userManager,
        ILogger<UserRepository> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<IdentityResult> RegisterUserAsync(User user, string password, string role)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            var roleAddingResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleAddingResult.Succeeded)
            {
                _logger.LogError("Error(s) occured while adding user {0} to role {1}: {2}", 
                    user.Id, role, String.Join(", ", result.Errors.Select(err => $"{err.Code}: {err.Description}")));
            }
        }
        
        return result;
    }

    public async Task<User> GetAsync(string login) =>
        await _userManager.FindByNameAsync(login);

    public async Task<ICollection<User>> GetRangeAsync(ICollection<string> coauthorIds) =>
        await _userManager.Users.Where(u => coauthorIds.Contains(u.Id)).ToListAsync();
}