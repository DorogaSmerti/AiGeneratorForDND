using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface INpcService
{
    Task<NpcStat> GenerateNpcAsync(NpcRequest npc);
}