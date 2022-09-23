using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(RegisterUserDTO registerUserDto);
    Task<UserTokenDTO> ValidateUserAsync(LoginUserDTO loginUserDto);
    Task<bool> UserExistsAsync(string id);
    Task<bool> ChangePasswordAsync(string id, ChangePasswordDTO changePasswordDto);
}