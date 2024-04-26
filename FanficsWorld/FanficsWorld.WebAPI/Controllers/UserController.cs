using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FanficsWorld.WebAPI.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator;

    public UserController(
        IUserService service,
        IValidator<ChangePasswordDto> changePasswordValidator)
    {
        _service = service;
        _changePasswordValidator = changePasswordValidator;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<SimpleUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(string? userName = null) =>
        Ok(await _service.GetSimpleUsersAsync(User.GetUserId()!, userName));

    [HttpGet("admin-page")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ServicePagedResultDto<AdminPanelUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersForAdminPage(string? searchTerm = null, int page = 1, int itemsPerPage = 20) =>
        Ok(await _service.GetUsersAdminPageAsync(searchTerm, page, itemsPerPage));
    
    [HttpPatch("change-password/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword(string userId, ChangePasswordDto changePasswordDto)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(changePasswordDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var userExists = await _service.UserExistsAsync(userId);
        if (!userExists)
        {
            return NotFound();
        }

        var changeResult = await _service.ChangePasswordAsync(userId, changePasswordDto);
        return changeResult.IsSuccess
            ? NoContent()
            : BadRequest("Error while changing a password!");
    }

    [HttpPatch("block-status/{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ServiceResultDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeBlockStatus(string id)
    {
        var blockResult = await _service.ChangeBlockStatusAsync(id);
        return blockResult.IsSuccess
            ? NoContent()
            : BadRequest(blockResult);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ServiceResultDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var deleteResult = await _service.DeleteUserAsync(id);
        return deleteResult.IsSuccess
            ? NoContent()
            : BadRequest(deleteResult);
    }
}