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
            var Json = JsonNode.Parse(line);
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

    private readonly Dictionary<string, string[]> _armorProficiencies = new()
    {
        { "Barbarian", ["light", "medium", "shield"] },
        { "Bard", ["light"] },
        { "Cleric", ["light", "medium", "shield"] },
        { "Druid", ["light", "medium", "shield"] }, // По лору немагическая, но по тегам так
        { "Fighter", ["light", "medium", "heavy", "shield"] },
        { "Monk", [] }, // Монахи голые бегают
        { "Paladin", ["light", "medium", "heavy", "shield"] },
        { "Ranger", ["light", "medium", "shield"] },
        { "Rogue", ["light"] },
        { "Sorcerer", [] }, // Тканевые робы — это не броня в терминах Фаундри
        { "Warlock", ["light"] },
        { "Wizard", [] }
    };

    private readonly Dictionary<string, string> _avatar = new()
    {
        { "Human Warrior ", "https://dnd.su/gallery/articles/81_3_1522773429_s.jpg"},
    };

    public string[] GetClassProficiencies(string npcClass)
        => _classLootPools.GetValueOrDefault(npcClass, ["gear"]);

    public string[] GetArmorProficiencies(string npcClass)
        => _armorProficiencies.GetValueOrDefault(npcClass, []);

    public List<JsonNode> GetItems() => _allItems;

}