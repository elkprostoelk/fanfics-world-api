using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(RegisterUserDTO registerUserDto);
    Task<UserTokenDTO> ValidateUserAsync(LoginUserDTO loginUserDto);
}