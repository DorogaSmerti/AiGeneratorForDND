using Microsoft.AspNetCore.Mvc;
using StoryTracker.Service.Interface;

namespace StoryTracker.Controller;

[ApiController]
[Route("api/[controller]")]

public class VectorSearchController(IAiService aiService, IVectorService vectorService) : ControllerBase
{
    [HttpGet("embedding")]
    public async Task<IActionResult> GetItems([FromQuery] string query)
    {
        var result = await aiService.GenerateEmbeddingAsync(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("buildEmbedding")]
    public async Task<IActionResult> BuildEmbedding([FromQuery] int limit)
    {
        var result = await vectorService.BuildDataBaseVectorAsync(limit);

        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}