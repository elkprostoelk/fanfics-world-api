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
    private readonly IValidator<EditFanficDto> _editFanficValidator;

    public FanficController(
        IFanficService service,
        IValidator<NewFanficDto> newFanficValidator,
        IValidator<EditFanficDto> editFanficValidator)
    {
        _service = service;
        _newFanficValidator = newFanficValidator;
        _editFanficValidator = editFanficValidator;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetFanficAsync(long id)
    {
        var fanfic = await _service.GetDisplayFanficByIdAsync(id);
        if (fanfic is null)
        {
            return NotFound($"Fanfic {id} was not found!");
        }

        return Ok(fanfic);
    }

    [HttpGet]
    public async Task<IActionResult> GetFanficsPageAsync(int page = 1, int itemsPerPage = 20)
    {
        if (page <= 0 || itemsPerPage <= 0)
        {
            return BadRequest("Pages' and items' count must be only positive!");
        }
        
        return Ok(await _service.GetPageWithFanficsAsync(page, itemsPerPage));
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
        
        var createdFanficId = await _service.CreateAsync(newFanficDto, User.GetUserId()!);
        
        return createdFanficId.HasValue
            ? StatusCode(201, createdFanficId)
            : BadRequest("An error occured while creating a fanfic");
    }

    [Authorize]
    [HttpPost("add-tags/{fanficId:long}")]
    public async Task<IActionResult> AddTagsAsync(long fanficId, AddTagsDto addTagsDto)
    {
        var fanfic = await _service.GetByIdAsync(fanficId);
        if (fanfic is null)
        {
            return NotFound($"Fanfic {fanficId} was not found!");
        }

        if (fanfic.Author.Id != User.GetUserId() && !User.IsInRole("Admin"))
        {
            return StatusCode(403, "You cannot add tags to the other user's fanfic!");
        }

        var added = await _service.AddTagsToFanficAsync(fanficId, addTagsDto);
        return added
            ? Ok("Tags are added!")
            : BadRequest("An error occured while adding tags to the fanfic");
    }

    [HttpPatch("increment-views/{id:long}")]
    public async Task<IActionResult> IncrementFanficViewsAsync(long id)
    {
        var fanfic = await _service.GetByIdAsync(id);
        if (fanfic is null)
        {
            return NotFound($"Fanfic {id} was not found!");
        }

        try
        {
            FanficViewsCounterMutex.WaitOne();
            var updatedCounter = await _service.IncrementFanficViewsCounterAsync(id);
            return updatedCounter.HasValue
                ? Ok(updatedCounter)
                : BadRequest($"Fanfic {id} views were not incremented!");
        }
        finally
        {
            FanficViewsCounterMutex.ReleaseMutex();
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditFanficAsync(EditFanficDto editFanficDto)
    {
        var validationResult = await _editFanficValidator.ValidateAsync(editFanficDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var fanfic = await _service.GetByIdAsync(editFanficDto.Id);
        if (fanfic is null)
        {
            return NotFound();
        }

        var edited = await _service.EditAsync(editFanficDto);
        return edited
            ? NoContent()
            : BadRequest();
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteFanficAsync(long id)
    {
        var fanfic = await _service.GetByIdAsync(id);
        if (fanfic is null)
        {
            return NotFound($"Fanfic {id} was not found!");
        }

        if (fanfic.Author.Id != User.GetUserId() && !User.IsInRole("Admin"))
        {
            return StatusCode(403, "You cannot delete other user's fanfic!");
        }

        var deleted = await _service.DeleteAsync(id);
        
        return deleted ? NoContent() : BadRequest("An error occured while deleting a fanfic");
    }
}