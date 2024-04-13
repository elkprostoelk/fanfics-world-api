using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers.Internal;

[ApiController]
[Route("api/internal/[controller]")]
public class FanficController : ControllerBase
{
    private readonly IFanficService _service;

    public FanficController(IFanficService service)
    {
        _service = service;
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(FanficDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        var fanfic = await _service.GetByIdAsync(id);
        if (fanfic is null)
        {
            return NotFound($"Fanfic {id} was not found!");
        }

        return Ok(fanfic);
    }
}