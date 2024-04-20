using FanficsWorld.Common.DTO;
using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<UserTokenDto?> ValidateUserAsync(LoginUserDto loginUserDto);
    Task<bool> UserExistsAsync(string idOrUserName);
    Task<ServiceResultDto> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
    Task<List<SimpleUserDto>> GetSimpleUsersAsync(string userId,
        string? userName = null);
    Task<ServicePagedResultDto<AdminPanelUserDto>> GetUsersAdminPageAsync(string? searchTerm, int page,
        int itemsPerPage);
    Task<ServiceResultDto> DeleteUserAsync(string id);
}