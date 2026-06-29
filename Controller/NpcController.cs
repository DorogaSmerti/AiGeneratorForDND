using Microsoft.AspNetCore.Mvc;
using StoryTracker.Models;
using StoryTracker.Service;

namespace StoryTracker.Controller;

[ApiController]
[Route("api/[controller]")]

public class NpcController : ControllerBase
{
    private readonly INpcService _npcService;

    public NpcController(INpcService npcService)
    {
        _npcService = npcService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> NpcGenerate([FromBody]NpcRequest npcRequest)
    {
        var result = await _npcService.GenerateNpcAsync(npcRequest);

        if(result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}