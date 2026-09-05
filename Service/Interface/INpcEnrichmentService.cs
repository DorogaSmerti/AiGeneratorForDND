using StoryTracker.Models;
using System.Text.Json.Nodes;
namespace StoryTracker.Service.Interface;

public interface INpcEnrichmentService
{
    void MappingInventory(BaseCharacter baseCharacter);
    string? GetAvatarFromDump(string? npcClass);
}