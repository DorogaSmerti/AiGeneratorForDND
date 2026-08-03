using StoryTracker.Models;
namespace StoryTracker.Service.Interface;

public interface INpcEnrichmentService
{
    Task MappingInventoryAsync(BaseCharacter baseCharacter);
    string? GetAvatarFromDump(string? npcClass);
}