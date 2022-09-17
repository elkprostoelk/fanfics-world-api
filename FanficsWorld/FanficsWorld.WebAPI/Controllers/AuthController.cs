using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(
        IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegisterUserDTO registerUserDto)
    {
        var registered = await _userService.RegisterUserAsync(registerUserDto);
        return registered ? StatusCode(201) : Conflict();
    }
}