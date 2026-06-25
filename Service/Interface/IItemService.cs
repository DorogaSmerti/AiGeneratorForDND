using System.Text.Json.Nodes;
using StoryTracker.Models;

namespace StoryTracker.Service;

public interface IItemService
{
    Task<JsonNode?> GetItemFromLocalDump(InventoryGenerationRequest inventoryTags);
}