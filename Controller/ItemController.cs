using Microsoft.AspNetCore.Mvc;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Controller;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpPost("parse")]
    public async Task<IActionResult> ParseItem([FromBody] InventoryGenerationRequest inventoryGenerationRequest)
    {
        var parsedItem = _itemService.GetItemFromLocalDump(inventoryGenerationRequest);
        if (!parsedItem.IsSuccess)
        {
            return BadRequest(parsedItem.Error);
        }

        return Ok(parsedItem.Value);
    }
}