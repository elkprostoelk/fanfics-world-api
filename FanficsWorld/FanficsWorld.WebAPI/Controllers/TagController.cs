using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _service;

    public TagController(ITagService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTagsAsync() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("top10")]
    public async Task<IActionResult> GetTop10TagsAsync() =>
        Ok(await _service.GetTop10Async());
}