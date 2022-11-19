using FanficsWorld.Common.DTO;

namespace FanficsWorld.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<UserTokenDto?> ValidateUserAsync(LoginUserDto loginUserDto);
    Task<bool> UserExistsAsync(string idOrUserName);
    Task<bool> ChangePasswordAsync(string id, ChangePasswordDto changePasswordDto);
    Task<ServicePagedResultDto<SimpleUserDto>> GetSimpleUsersChunkAsync(int chunkNumber, int chunkSize, string userId);
}