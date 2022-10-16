using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<UserTokenDto?> ValidateUserAsync(LoginUserDto loginUserDto);
    Task<bool> UserExistsAsync(string id);
    Task<bool> ChangePasswordAsync(string id, ChangePasswordDto changePasswordDto);
}