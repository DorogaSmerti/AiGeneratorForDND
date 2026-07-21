using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface INpcService
{
    Task<Result<BaseCharacter>> GenerateNpcAsync(NpcRequest npc);
    Task<Result<MerchantShop>> GenerateMerchantAsync(MerchantRequest npc);
}