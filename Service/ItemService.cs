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

        var allowedPoolNames = _storage.GetClassProficiencies(inventoryTags.ClassName); //получаем пул из типов оружия по имени класса

        foreach(var i in allowedPoolNames) Console.WriteLine(i);

        var chosenPoolName = allowedPoolNames[random.Next(allowedPoolNames.Length)]; // берем рандомный тип из пула

        var items = _storage.GetItems(); // берем все предметы тк инкапсуляция и нужно через метод

        var suitableItems = items.Where(item => 
        string.Equals((string?)item["system"]?["rarity"], inventoryTags.Rarity, StringComparison.OrdinalIgnoreCase)
        ).ToList();
        
        if(suitableItems.Count == 0) return Task.FromResult<JsonNode?>(null); 

        Console.WriteLine(suitableItems.Count);

        return Task.FromResult(suitableItems[random.Next(suitableItems.Count)]); // возвращаем рандомный предмет из списка подходящик предметов

    }
}