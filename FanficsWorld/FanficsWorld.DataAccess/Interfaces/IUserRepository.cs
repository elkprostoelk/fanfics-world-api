using FanficsWorld.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IUserRepository
{
    Task<IdentityResult> RegisterUserAsync(User user, string password, string role);
    Task<User?> GetAsync(string login);
    Task<ICollection<User>> GetRangeAsync(ICollection<string> coauthorIds);
    Task<ICollection<User>> GetChunkAsync(int chunkNumber, int chunkSize);
    Task<long> CountAsync(string? currentUserId);
}