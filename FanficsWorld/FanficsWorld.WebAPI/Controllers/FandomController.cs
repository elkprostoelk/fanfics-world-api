using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FandomController : ControllerBase
{
    private readonly IFandomService _service;
    private readonly IValidator<NewFandomDto> _newFandomValidator;

    public FandomController(IFandomService service,
        IValidator<NewFandomDto> newFandomValidator)
    {
        _service = service;
        _newFandomValidator = newFandomValidator;
    }

    [HttpGet("top10")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTop10FandomsAsync() => 
        Ok(await _service.GetTop10FandomsAsync());

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFandomsByTitleAsync([FromQuery]string title) =>
        Ok(await _service.SearchByTitleAsync(title));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFandomAsync(NewFandomDto newFandomDto)
    {
        var validationResult = await _newFandomValidator.ValidateAsync(newFandomDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var createResult = await _service.CreateAsync(newFandomDto);
        
        return createResult.IsSuccess
            ? StatusCode(201, createResult.Result)
            : BadRequest(createResult);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFandomAndFanficsAsync(long id)
    {
        var fandom = await _service.GetFandomWithFanficsAsync(id);
        if (fandom is null)
        {
            return NotFound($"Fandom {id} was not found!");
        }

        return Ok(fandom);
    }
}