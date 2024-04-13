﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace FanficsWorld.Services.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository repository,
        IMapper mapper,
        IConfiguration configuration,
        ILogger<UserService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        var user = _mapper.Map<User>(registerUserDto);
        return await _repository.RegisterUserAsync(
            user,
            registerUserDto.Password,
            registerUserDto.Role);
    }

    public async Task<UserTokenDto?> ValidateUserAsync(LoginUserDto loginUserDto)
    {
        var user = await _repository.GetAsync(loginUserDto.Login);
        if (user is null)
        {
            return null;
        }
        
        var passwordValid = await _repository.CheckPasswordAsync(user, loginUserDto.Password);
        return passwordValid
            ? new UserTokenDto {Jwt = await GenerateTokenAsync(user)}
            : null;
    }

    public async Task<bool> UserExistsAsync(string idOrUserName) =>
        await _repository.UserExistsAsync(idOrUserName);

    public async Task<ServiceResultDto> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
    {
        var user = await _repository.GetAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("Cannot change a password. User {UserId} does not exist.", userId);
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = "User has not been found!"
            };
        }

        var result = await _repository.ChangePasswordAsync(
            user,
            changePasswordDto.CurrentPassword,
            changePasswordDto.NewPassword);
        
        return new ServiceResultDto
        {
            IsSuccess = result.Succeeded,
            ErrorMessage = string.Join("; ", result.Errors.Select(e => e.Description))
        };
    }

    public async Task<List<SimpleUserDto>> GetSimpleUsersAsync(string userId, string? userName = null)
    {
        var users = await _repository.GetListAsync(userName);
        users = (!string.IsNullOrWhiteSpace(userId) ? users.Where(u => u.Id != userId) : users).ToList();
        return _mapper.Map<List<SimpleUserDto>>(users);
    }

    private async Task<string> GenerateTokenAsync(User user)
    {
        var jwtConfig = _configuration.GetSection("JwtConfig");
        var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]!);
        var secret = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Name, user.UserName!)
        };
        var roles = await _repository.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var jwtSettings = _configuration.GetSection("JwtConfig");
        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings["ValidIssuer"],
            audience: jwtSettings["ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddHours(Convert.ToDouble(jwtSettings["ExpiresIn"])),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}