using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<LoginUserDTO> _loginValidator;
    private readonly IValidator<RegisterUserDTO> _registerValidator;
    private readonly IValidator<ChangePasswordDTO> _changePasswordValidator;

    public AuthController(
        IUserService userService,
        IValidator<LoginUserDTO> loginValidator,
        IValidator<RegisterUserDTO> registerValidator,
        IValidator<ChangePasswordDTO> changePasswordValidator)
    {
        _userService = userService;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _changePasswordValidator = changePasswordValidator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDTO loginUserDto)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginUserDto);
        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(failure => ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage));
            return BadRequest(ModelState);
        }
        
        var validatedUserTokenDTO = await _userService.ValidateUserAsync(loginUserDto);
        return validatedUserTokenDTO is not null ? Ok(validatedUserTokenDTO) : Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserDTO registerUserDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerUserDto);
        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(failure => ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage));
            return BadRequest(ModelState);
        }
        
        var registered = await _userService.RegisterUserAsync(registerUserDto);
        return registered ? StatusCode(201) : Conflict();
    }

    [Authorize]
    [HttpPatch("change-password/{id}")]
    public async Task<IActionResult> ChangePassword(string id, ChangePasswordDTO changePasswordDto)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(changePasswordDto);
        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(failure => ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage));
            return BadRequest(ModelState);
        }
        
        var userExists = await _userService.UserExistsAsync(id);
        if (!userExists)
        {
            return NotFound();
        }

        var changed = await _userService.ChangePasswordAsync(id, changePasswordDto);
        return changed ? NoContent() : Conflict("Error while changing a password!");
    }
}