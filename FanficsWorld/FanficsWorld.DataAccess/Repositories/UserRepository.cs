using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IdentityResult> RegisterUserAsync(User user, string password) =>
        await _userManager.CreateAsync(user, password);
}