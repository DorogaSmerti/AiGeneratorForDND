using System.Text.Json.Nodes;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class ItemService(IItemDataStorage _storage, ILogger<ItemService> _logger) : IItemService
{

    public async Task<Result<JsonNode?>> GetItemFromLocalDump(InventoryGenerationRequest inventoryTags)
    {
        Random random = new Random();

        var allowedPoolNames = _storage.GetClassProficiencies(inventoryTags.ClassName);

        string? chosenPoolName = null;
        if(string.IsNullOrWhiteSpace(inventoryTags.Type))
            chosenPoolName = allowedPoolNames[random.Next(allowedPoolNames.Length)];
        else
            chosenPoolName = inventoryTags.Type;

        var items = _storage.GetItems();

        _logger.LogInformation("chosen pool: {pool}", chosenPoolName);

        var suitableItems = items.Where(item =>
            string.Equals((string?)item["system"]?["rarity"], inventoryTags.Rarity, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        suitableItems = suitableItems.Where(item => 
            (item["system"]?["properties"]?.AsArray()?.Any(prop =>
                string.Equals((string?)prop, chosenPoolName, StringComparison.OrdinalIgnoreCase)
            ) ?? false) 
            || 
            string.Equals((string?)item["type"], chosenPoolName, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        _logger.LogInformation("suitable items count: {Count}", suitableItems.Count);

        if(suitableItems.Count == 0) return Result<JsonNode?>.Failure(DomainErrors.Item.NotFound);

        var chosenItem = suitableItems[random.Next(suitableItems.Count)];
        chosenItem!["system"]!["quantity"] = random.Next(1,10);

        return Result<JsonNode?>.Success(chosenItem);
    }
}