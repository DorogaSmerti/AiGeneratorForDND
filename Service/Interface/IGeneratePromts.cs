using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface IGeneratePromts
{
    string GenerateNpc(NpcRequest npc);
    string GenerateMerchant(MerchantRequest merchantRequest);
}