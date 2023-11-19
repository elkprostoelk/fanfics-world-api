using FanficsWorld.Services.Interfaces;
using FanficsWorld.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers.v2;

[ApiController]
[Route("api/v2/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    private readonly IUserService _service = service;

    [HttpGet("get-chunk/{number:int}/{size:int}")]
    [Authorize]
    public async Task<IActionResult> GetUsersChunkByUserName([FromQuery] string? userName, int number=0, int size=10) =>
        Ok(await _service.GetSimpleUsersChunkAsync(number, size, User.GetUserId()!, userName));
}