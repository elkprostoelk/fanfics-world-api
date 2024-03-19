using FanficsWorld.Services.Interfaces;
using FanficsWorld.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersAsync(string? userName = null) =>
        Ok(await _service.GetSimpleUsersAsync(User.GetUserId()!, userName));
}