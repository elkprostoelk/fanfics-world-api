using FanficsWorld.Services.Interfaces;
using FanficsWorld.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("get-chunk/{number:int}/{size:int}")]
    [Authorize]
    public async Task<IActionResult> GetUsersAsync(int number=0, int size=10) =>
        Ok(await _service.GetSimpleUsersChunkAsync(number, size, User.GetUserId()!));
}