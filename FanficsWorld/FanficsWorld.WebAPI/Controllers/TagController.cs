using FanficsWorld.Common.DTO;
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
    [ProducesResponseType(typeof(List<TagDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTagsAsync() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("top10")]
    [ProducesResponseType(typeof(List<TagDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTop10TagsAsync() =>
        Ok(await _service.GetTop10Async());

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<TagDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTagsByTitleAsync(string title) =>
        Ok(await _service.GetAllAsync(title));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TagWithFanficsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTagWithFanfics(long id)
    {
        var tag = await _service.GetFullByIdAsync(id);
        if (tag is null)
        {
            return NotFound($"Tag {id} was not found!");
        }

        return Ok(tag);
    }
}