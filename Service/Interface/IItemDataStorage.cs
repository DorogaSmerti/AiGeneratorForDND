using System.Text.Json.Nodes;

namespace StoryTracker.Service.Interface;

public interface IItemDataStorage
{
    string[] GetClassProficiencies(string npcClass);
    List<JsonNode> GetItems();
}