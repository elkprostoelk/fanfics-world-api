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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace FanficsWorld.Services.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public UserService(
        IUserRepository repository,
        IMapper mapper,
        ILogger<UserService> logger,
        IConfiguration configuration,
        UserManager<User> userManager)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        var user = _mapper.Map<User>(registerUserDto);
        try
        {
            var result = await _repository.RegisterUserAsync(user, registerUserDto.Password, registerUserDto.Role);
            return result.Succeeded;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return false;
        }
    }

    public async Task<UserTokenDto?> ValidateUserAsync(LoginUserDto loginUserDto)
    {
        try
        {
            var user = await _repository.GetAsync(loginUserDto.Login);
            if (user is null)
            {
                return null;
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            return passwordValid ? new UserTokenDto {Jwt = await GenerateTokenAsync(user)} : null;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return null;
        }
    }

    public async Task<bool> UserExistsAsync(string idOrUserName) => 
        await _userManager.FindByIdAsync(idOrUserName) is not null
        || await _userManager.FindByNameAsync(idOrUserName) is not null;

    public async Task<bool> ChangePasswordAsync(string id, ChangePasswordDto changePasswordDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            var result =
                await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,
                    changePasswordDto.NewPassword);
            return result.Succeeded;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return false;
        }
    }

    public async Task<ICollection<SimpleUserDto>> GetSimpleUsersChunkAsync(int chunkNumber, int chunkSize)
    {
        try
        {
            var users = await _repository.GetChunkAsync(chunkNumber, chunkSize);
            return _mapper.Map<ICollection<SimpleUserDto>>(users);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "An exception occured while executing the service");
            return new List<SimpleUserDto>();
        }
    }

    private async Task<string> GenerateTokenAsync(User user)
    {
        var jwtConfig = _configuration.GetSection("JwtConfig");
        var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
        var secret = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Name, user.UserName)
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