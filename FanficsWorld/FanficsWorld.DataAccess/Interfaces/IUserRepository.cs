﻿using FanficsWorld.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IUserRepository
{
    Task<IdentityResult> RegisterUserAsync(User user, string password);
}