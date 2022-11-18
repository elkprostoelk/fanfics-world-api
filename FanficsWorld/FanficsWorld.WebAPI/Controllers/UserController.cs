using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("{number:int}/{size:int}")]
    [Authorize]
    public async Task<IActionResult> GetUsersAsync(int number, int size) =>
        Ok(await _service.GetSimpleUsersChunkAsync(number, size));
}