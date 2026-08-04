using Microsoft.AspNetCore.Mvc;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Controller;

[ApiController]
[Route("api/[controller]")]

public class FactionController : ControllerBase
{
    private readonly IFactionService _factionService;

    public FactionController(IFactionService factionService)
    {
        _factionService = factionService;
    }

    [HttpPost]
    public async Task<IActionResult> GenerateFaction([FromBody] FactionRequest factionRequest)
    {
        var result = await _factionService.GenerateFactionAsync(factionRequest);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}