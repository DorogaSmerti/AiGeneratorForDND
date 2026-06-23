using StoryTracker.Models;

namespace StoryTracker.Service;

public interface IGeneratePromts
{
    string GenerateNpc(NpcRequest npc);
}