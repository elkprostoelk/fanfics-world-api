using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IUserService userService,
    IValidator<LoginUserDto> loginValidator,
    IValidator<RegisterUserDto> registerValidator,
    IValidator<ChangePasswordDto> changePasswordValidator) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IValidator<LoginUserDto> _loginValidator = loginValidator;
    private readonly IValidator<RegisterUserDto> _registerValidator = registerValidator;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator = changePasswordValidator;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginUserDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var userExists = await _userService.UserExistsAsync(loginUserDto.Login);
        if (!userExists)
        {
            return NotFound($"User {loginUserDto.Login} was not found!");
        }
        
        var validatedUserTokenDto = await _userService.ValidateUserAsync(loginUserDto);
        return validatedUserTokenDto is not null
            ? Ok(validatedUserTokenDto)
            : Unauthorized("Password is invalid!");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserDto registerUserDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerUserDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var registered = await _userService.RegisterUserAsync(registerUserDto);
        return registered.Succeeded
            ? StatusCode(201)
            : BadRequest("An error occured while registering the user");
    }

    [Authorize]
    [HttpPatch("change-password/{id}")]
    public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto changePasswordDto)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(changePasswordDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var userExists = await _userService.UserExistsAsync(id);
        if (!userExists)
        {
            return NotFound();
        }

        var changed = await _userService.ChangePasswordAsync(id, changePasswordDto);
        return changed
            ? NoContent()
            : BadRequest("Error while changing a password!");
    }
}