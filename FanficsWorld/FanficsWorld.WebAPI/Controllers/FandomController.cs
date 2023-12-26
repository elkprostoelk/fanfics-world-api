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
    public async Task<IActionResult> GetTop10FandomsAsync() => 
        Ok(await _service.GetTop10FandomsAsync());

    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> GetFandomsByTitleAsync([FromQuery]string title) =>
        Ok(await _service.SearchByTitleAsync(title));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateFandomAsync(NewFandomDto newFandomDto)
    {
        var validationResult = await _newFandomValidator.ValidateAsync(newFandomDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var createdFandomId = await _service.CreateAsync(newFandomDto);
        
        return createdFandomId.HasValue
            ? StatusCode(201, createdFandomId)
            : BadRequest("An error occured while creating a fandom");
    }

    [HttpGet("{id:long}")]
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