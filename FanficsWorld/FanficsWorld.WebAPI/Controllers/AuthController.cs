using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(
        IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDTO loginUserDto)
    {
        var validatedUserTokenDTO = await _userService.ValidateUserAsync(loginUserDto);
        return validatedUserTokenDTO is not null ? Ok(validatedUserTokenDTO) : Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserDTO registerUserDto)
    {
        var registered = await _userService.RegisterUserAsync(registerUserDto);
        return registered ? StatusCode(201) : Conflict();
    }

    [Authorize]
    [HttpPatch("change-password/{id}")]
    public async Task<IActionResult> ChangePassword(string id, ChangePasswordDTO changePasswordDto)
    {
        var userExists = await _userService.UserExistsAsync(id);
        if (!userExists)
        {
            return NotFound();
        }

        var changed = await _userService.ChangePasswordAsync(id, changePasswordDto);
        return changed ? NoContent() : Conflict("Error while changing a password!");
    }
}