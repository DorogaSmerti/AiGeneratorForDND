using System.Text.Json.Nodes;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class ItemDataStorage : IItemDataStorage
{
    private readonly IConfiguration _configuration;
    private readonly List<JsonNode> _allItems = new();

    public ItemDataStorage(IConfiguration configuration)
    {
        _configuration = configuration;

        string allItemsPath = _configuration["LocalDumpSettings:AllItemsPath"] ?? throw new InvalidOperationException();

        LoadFileToPool(allItemsPath, _allItems);

    }

    private static void LoadFileToPool(string path, List<JsonNode> node)
    {
        var lines = File.ReadLines(path);

        foreach(var line in lines)
        {
            if(string.IsNullOrWhiteSpace(line)) continue;

            var Json = JsonNode.Parse(line);

            if(Json == null) continue;

            node.Add(Json);
        }
    }

    private readonly Dictionary<string, string[]> _classLootPools = new()
    {
        { "Wizard", ["mgc", "consumable"] },
        { "Sorcerer", ["mgc", "consumable"] },
        { "Warlock", ["mgc", "consumable"] },
        
        { "Rogue", ["fin", "consumable"] },
        { "Monk", ["fin", "consumable"] },
        
        { "Fighter", ["hvy", "weapon", "equipment"] },
        { "Barbarian", ["hvy", "weapon", "consumable"] },
        { "Paladin", ["hvy", "equipment", "mgc"] },
        
        { "Cleric", ["consumable", "mgc", "equipment"] },
        { "Bard", ["mgc", "fin", "consumable"] },
        { "Druid", ["consumable", "consumable", "mgc"] },
        { "Ranger", ["fin", "weapon", "equipment"] },

        { "Alchemist", ["consumable"] },
        { "Blacksmith", ["weapon", "equipment"] },
        { "Merchant_Magic", ["mgc", "consumable"] },
        { "Merchant_General", ["loot", "equipment"] }
    };

    public string[] GetClassProficiencies(string npcClass)
        => _classLootPools.GetValueOrDefault(npcClass, ["gear"]);

    public List<JsonNode> GetItems() => _allItems;

}