using Microsoft.AspNetCore.Mvc;
using StoryTracker.Models;
using StoryTracker.Service;

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
        var parsedItem = await _itemService.GetItemFromLocalDump(inventoryGenerationRequest);
        if (parsedItem == null)
        {
            return NotFound();
        }

        return Ok(parsedItem);
    }
}