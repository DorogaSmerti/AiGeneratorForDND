using System.Text.Json.Nodes;
using StoryTracker.Models;

namespace StoryTracker.Service;

public class ItemService : IItemService
{
    private readonly IITemDataStorage _storage;

    public ItemService(IITemDataStorage storage)
    {
        _storage = storage;
    }

    public Task<JsonNode?> GetItemFromLocalDump(InventoryGenerationRequest inventoryTags)
    {
        Random random = new Random();

        var allowedPoolNames = _storage.GetClassProficiencies(inventoryTags.ClassName);

        var chosenPoolName = allowedPoolNames[random.Next(allowedPoolNames.Length)];
        var items = _storage.GetItems();

        var suitableItems = items.Where(item => 
        string.Equals((string?)item["system"]?["rarity"], inventoryTags.Rarity, StringComparison.OrdinalIgnoreCase)
        ).ToList();
        
        if(suitableItems.Count == 0) return Task.FromResult<JsonNode?>(null); 

        return Task.FromResult(suitableItems[random.Next(suitableItems.Count)]);

    }
}