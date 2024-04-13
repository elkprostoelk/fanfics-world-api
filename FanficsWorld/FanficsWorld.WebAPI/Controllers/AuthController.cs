using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<LoginUserDto> _loginValidator;
    private readonly IValidator<RegisterUserDto> _registerValidator;

    public AuthController(IUserService userService,
        IValidator<LoginUserDto> loginValidator,
        IValidator<RegisterUserDto> registerValidator)
    {
        _userService = userService;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
}