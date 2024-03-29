﻿using FanficsWorld.Common.DTO;
using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<UserTokenDto?> ValidateUserAsync(LoginUserDto loginUserDto);
    Task<bool> UserExistsAsync(string idOrUserName);
    Task<bool> ChangePasswordAsync(string id, ChangePasswordDto changePasswordDto);
    Task<List<SimpleUserDto>> GetSimpleUsersAsync(string userId,
        string? userName = null);
}