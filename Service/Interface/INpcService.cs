using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface INpcService
{
    Task<BaseCharacter> GenerateNpcAsync(NpcRequest npc);
    Task<MerchantShop> GenerateMerchantAsync(MerchantRequest npc);
}