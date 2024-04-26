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
    [ProducesResponseType(typeof(FanficPageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFanfic(long id)
    {
        var fanfic = await _service.GetDisplayFanficByIdAsync(id);
        if (fanfic is null)
        {
            return NotFound($"Fanfic {id} was not found!");
        }

        return Ok(fanfic);
    }

    [HttpGet]
    [Obsolete("Use v2 Search endpoint instead")]
    [ProducesResponseType(typeof(ServicePagedResultDto<SimpleFanficDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFanficsPage(int page = 1, int itemsPerPage = 20)
    {
        if (page <= 0 || itemsPerPage <= 0)
        {
            return BadRequest("Pages' and items' count must be only positive!");
        }
        
        return Ok(await _service.GetPageWithFanficsAsync(page, itemsPerPage));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(long?), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFanfic(NewFanficDto newFanficDto)
    {
        var validationResult = await _newFanficValidator.ValidateAsync(newFanficDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var createResult = await _service.CreateAsync(newFanficDto, User.GetUserId());
        
        return createResult.IsSuccess
            ? StatusCode(201, createResult.Result)
            : BadRequest(createResult);
    }

    [HttpPatch("increment-views/{id:long}")]
    public async Task<IActionResult> IncrementFanficViews(long id)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditFanfic(EditFanficDto editFanficDto)
    {
        var validationResult = await _editFanficValidator.ValidateAsync(editFanficDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var editResult = await _service.EditAsync(editFanficDto);
        return editResult.IsSuccess
            ? NoContent()
            : BadRequest(editResult);
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteFanfic(long id)
    {
        var deleteResult = await _service.DeleteAsync(id);
        
        return deleteResult.IsSuccess
            ? NoContent()
            : BadRequest(deleteResult);
    }
}