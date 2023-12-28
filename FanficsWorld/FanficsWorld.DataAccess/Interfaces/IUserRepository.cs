﻿using FanficsWorld.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IUserRepository
{
    Task<IdentityResult> RegisterUserAsync(User user, string password, string role);
    Task<User?> GetAsync(string idOrUserName);
    Task<ICollection<User>> GetRangeAsync(ICollection<string> coauthorIds);
    Task<ICollection<User>> GetChunkAsync(string? userName, int chunkNumber, int chunkSize);
    Task<long> CountAsync(string? currentUserId);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<bool> UserExistsAsync(string idOrUserName);
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    Task<ICollection<string>> GetRolesAsync(User user);
}