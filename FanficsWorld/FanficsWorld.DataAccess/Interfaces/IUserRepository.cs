using FanficsWorld.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IUserRepository
{
    Task<IdentityResult> RegisterUserAsync(User user, string password, string role);
    Task<User?> GetAsync(string idOrUserName, bool asNoTracking = true);
    Task<List<User>> GetRangeAsync(List<string> userIds);
    Task<List<User>> GetListAsync(string? userName);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<bool> UserExistsAsync(string idOrUserName);
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    Task<List<string>> GetRolesAsync(User user);
    Task<IdentityResult> UpdateAsync(User user);
    Task<bool> DeleteAsync(User user);
    IQueryable<User> GetAll();
}