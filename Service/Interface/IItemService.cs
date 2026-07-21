using System.Text.Json.Nodes;
using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface IItemService
{
    Task<Result<JsonNode?>> GetItemFromLocalDump(InventoryGenerationRequest inventoryTags);
}