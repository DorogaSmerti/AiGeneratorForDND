using Microsoft.AspNetCore.Mvc;
using StoryTracker.Service.Interface;

namespace StoryTracker.Controller;

[ApiController]
[Route("api/[controller]")]

public class VectorSearchController(IVectorService vectorService) : ControllerBase
{
    [HttpGet("getItem")]
    public async Task<IActionResult> GetItem([FromQuery] string query, CancellationToken cancellationToken = default)
    {
        var result = await vectorService.SearchAsync(query, cancellationToken);

        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("buildEmbedding")]
    public async Task<IActionResult> BuildVectorDataBaseAsync(CancellationToken cancellationToken = default)
    {
        var result = await vectorService.BuildVectorDataBaseAsync(cancellationToken);

        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}