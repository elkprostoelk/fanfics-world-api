using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FanficsWorld.Services.Services;

public class UserService(
    IUserRepository repository,
    IMapper mapper,
    IConfiguration configuration,
    UserManager<User> userManager) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<User> _userManager = userManager;

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
        var passwordValid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
        return passwordValid
            ? new UserTokenDto {Jwt = await GenerateTokenAsync(user)}
            : null;
    }

    public async Task<bool> UserExistsAsync(string idOrUserName) => 
        await _userManager.FindByIdAsync(idOrUserName) is not null
        || await _userManager.FindByNameAsync(idOrUserName) is not null;

    public async Task<bool> ChangePasswordAsync(string id, ChangePasswordDto changePasswordDto)
    {
        var user = await _repository.GetAsync(id);
        if (user is null)
        {
            return false;
        }

        var result = await _userManager.ChangePasswordAsync(
            user,
            changePasswordDto.CurrentPassword,
            changePasswordDto.NewPassword);
        return result.Succeeded;
    }

    public async Task<ServicePagedResultDto<SimpleUserDto>> GetSimpleUsersChunkAsync(int chunkNumber, int chunkSize,
        string userId, string? userName)
    {
        var users = await _repository.GetChunkAsync(userName, chunkNumber, chunkSize);
        var dtos = _mapper.Map<ICollection<SimpleUserDto>>(users.Where(u => u.Id != userId));
        var usersCount = await _repository.CountAsync(userId);
        return new ServicePagedResultDto<SimpleUserDto>
        {
            PageContent = dtos,
            CurrentPage = chunkNumber + 1,
            ItemsPerPage = chunkSize,
            TotalItems = usersCount,
            PagesCount = Convert.ToInt32(Math.Ceiling(usersCount / (double)chunkSize))
        };
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
        var roles = await _userManager.GetRolesAsync(user);
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