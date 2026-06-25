using StoryTracker.Models;

namespace StoryTracker.Service;

public interface INpcService
{
    Task<NpcStat> GenerateNpcAsync(NpcRequest npc);
}