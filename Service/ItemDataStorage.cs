using System.Text.Json.Nodes;

namespace StoryTracker.Service;

public class ItemDataStorage : IITemDataStorage
{
    private readonly IConfiguration _configuration;
    private readonly List<JsonNode> _allItems = new();

    public ItemDataStorage(IConfiguration configuration)
    {
        _configuration = configuration;

        string allItemsPath = _configuration["LocalDumpSettings:AllItemsPath"] ?? throw new InvalidOperationException();

        LoadFileToPool(allItemsPath, _allItems);

    }

    private void LoadFileToPool(string path, List<JsonNode> node)
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
        { "Wizard", ["magic", "consumables"] },       // Магу — магические побрякушки и зелья/свитки
        { "Sorcerer", ["magic", "potion"] },
        { "Warlock", ["magic", "potion"] },
        
        { "Rogue", ["gear", "weapon"] },         // Вору — воровские инструменты (gear) или скрытное оружие
        { "Fighter", ["weapon", "armor"] },      // Воину — трофейное оружие или элементы доспехов
        { "Barbarian", ["weapon", "gear"] },
        
        { "Cleric", ["potion", "magic"] },       // Жрецу — хилки и святые артефакты
        { "Paladin", ["armor", "weapon"] },
        
        { "Bard", ["magic", "gear"] },           // Барду — музыкальные инструменты (gear) или магия
        { "Druid", ["potion", "gear"] },
        { "Monk", ["gear", "weapon"] },
        { "Ranger", ["weapon", "gear"] }
    };

    public string[] GetClassProficiencies(string npcClass)
        => _classLootPools.GetValueOrDefault(npcClass, ["gear"]);

    public List<JsonNode> GetItems() => _allItems;

}