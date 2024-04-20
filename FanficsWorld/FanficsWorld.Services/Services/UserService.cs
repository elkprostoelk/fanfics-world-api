using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public async Task<ServicePagedResultDto<AdminPanelUserDto>> GetUsersAdminPageAsync(
        string? searchTerm,
        int page,
        int itemsPerPage)
    {
        if (page <= 0 || itemsPerPage <= 0)
        {
            return new ServicePagedResultDto<AdminPanelUserDto>();
        }

        var usersQuery = _repository.GetAll();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            usersQuery = usersQuery.Where(u => u.Id == searchTerm
                                               || (u.UserName != null && u.UserName.Contains(searchTerm))
                                               || (u.Email != null && u.Email.Contains(searchTerm)));
        }

        var totalUsersCount = await usersQuery.CountAsync();
        var userDtoList = await usersQuery
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .Select(u => new AdminPanelUserDto
            {
                Id = u.Id,
                Email = u.Email ?? string.Empty,
                UserName = u.UserName ?? string.Empty,
                RegistrationDate = u.RegistrationDate,
                DateOfBirth = u.DateOfBirth,
                FanficsCount = u.Fanfics.Count,
                CoauthoredFanficsCount = u.CoauthoredFanfics.Count
            })
            .ToListAsync();

        return new ServicePagedResultDto<AdminPanelUserDto>
        {
            PageContent = userDtoList,
            ItemsPerPage = itemsPerPage,
            CurrentPage = page,
            TotalItems = totalUsersCount,
            PagesCount = Convert.ToInt32(Math.Ceiling(totalUsersCount / (float)itemsPerPage))
        };
    }

    public async Task<ServiceResultDto> DeleteUserAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            _logger.LogWarning("Cannot recognize a user ID.");
            
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = $"User ID was not specified!"
            };
        }
        
        var user = await _repository.GetAsync(id);
        if (user is null)
        {
            _logger.LogWarning("Cannot delete a user {UserId}. It was not found.", id);
            
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = $"User {id} does not exist!"
            };
        }

        if (IsRootAdmin(user))
        {
            _logger.LogWarning("It is forbidden to delete an admin user account.");
            return new ServiceResultDto
            {
                IsSuccess = false,
                ErrorMessage = "It is forbidden to delete an admin user account!"
            };
        }

        var deleted = await _repository.DeleteAsync(user);
        return new ServiceResultDto
        {
            IsSuccess = deleted,
            ErrorMessage = deleted ? null : "Failed to delete a user!"
        };
    }

    private bool IsRootAdmin(User user)
    {
        return user.UserName == _configuration["AdminSettings:UserName"]
            && user.Email == _configuration["AdminSettings:Email"];
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