using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FanficsWorld.WebAPI.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FanficController : ControllerBase
{
    private readonly IFanficService _service;
    private readonly IValidator<NewFanficDto> _newFanficValidator;
    private static readonly Mutex FanficViewsCounterMutex = new();

    public FanficController(
        IFanficService service,
        IValidator<NewFanficDto> newFanficValidator)
    {
        _service = service;
        _newFanficValidator = newFanficValidator;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetFanficAsync(long id)
    {
        var fanfic = await _service.GetByIdAsync(id);
        if (fanfic is null)
        {
            return NotFound();
        }

        return Ok(fanfic);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateFanficAsync(NewFanficDto newFanficDto)
    {
        var validationResult = await _newFanficValidator.ValidateAsync(newFanficDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var created = await _service.CreateAsync(newFanficDto, User.GetUserId());
        
        return created ? StatusCode(201) : Conflict();
    }

    [Authorize]
    [HttpPost("add-tags/{fanficId:long}")]
    public async Task<IActionResult> AddTagsAsync(long fanficId, AddTagsDto addTagsDto)
    {
        var fanfic = await _service.GetByIdAsync(fanficId);
        if (fanfic is null)
        {
            return NotFound();
        }

        if (fanfic.Author.Id != User.GetUserId() && !User.IsInRole("Admin"))
        {
            return StatusCode(403, "You cannot add tags to the other user's fanfic!");
        }

        var added = await _service.AddTagsToFanficAsync(fanficId, addTagsDto);
        return added ? Ok() : Conflict();
    }

    [HttpPatch("increment-views/{id:long}")]
    public async Task<IActionResult> IncrementFanficViewsAsync(long id)
    {
        var fanfic = await _service.GetByIdAsync(id);
        if (fanfic is null)
        {
            return NotFound();
        }

        try
        {
            FanficViewsCounterMutex.WaitOne();
            var updatedCounter = await _service.IncrementFanficViewsCounterAsync(id);
            return updatedCounter.HasValue ? Ok(updatedCounter) : Conflict();
        }
        finally
        {
            FanficViewsCounterMutex.ReleaseMutex();
        }
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteFanficAsync(long id)
    {
        var fanfic = await _service.GetByIdAsync(id);
        if (fanfic is null)
        {
            return NotFound();
        }

        if (fanfic.Author.Id != User.GetUserId() && !User.IsInRole("Admin"))
        {
            return StatusCode(403, "You cannot delete other user's fanfic!");
        }

        var deleted = await _service.DeleteAsync(id);
        
        return deleted ? NoContent() : Conflict();
    }
}