using System.Text.Json.Nodes;

namespace StoryTracker.Service;

public interface IITemDataStorage
{
    string[] GetClassProficiencies(string npcClass);
    string[] GetArmorProficiencies(string npcClass);
    List<JsonNode> GetItems();
}