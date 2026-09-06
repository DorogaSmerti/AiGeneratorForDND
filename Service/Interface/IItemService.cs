using System.Text.Json.Nodes;
using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface IItemService
{
    Result<JsonNode?> GetItemFromLocalDump(InventoryGenerationRequest inventoryTags);
    Result<JsonNode> GetItemById(string id);
}