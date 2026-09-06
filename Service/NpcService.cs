using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class NpcService(
    IAiService aiService,
    IItemService itemService,
    IGeneratePromts generatePromts,
    IConfiguration configuration,
    ILogger<NpcService> logger
    ) : INpcService
{
    private readonly string _baseAvatarUrlPath = configuration["AvatarSettings:AvatarPath"]
        ?? throw new ArgumentNullException("UrlPath configuration is missing.");
    public async Task<Result<BaseCharacter>> GenerateNpcAsync(NpcRequest npcRequest)
    {
        if (npcRequest == null) return Result<BaseCharacter>.Failure(DomainErrors.Gpt.InvalidRequest);

        string prompt = generatePromts.GenerateNpc(npcRequest);

        ResponseSchema schema = AiSchemaBuilder.BuildSchemaForNpc();

        return await GenerateCharacterAsync<BaseCharacter>(prompt, schema);
    }

    public async Task<Result<MerchantShop>> GenerateMerchantAsync(MerchantRequest merchantRequest)
    {
        if (merchantRequest == null) return Result<MerchantShop>.Failure(DomainErrors.Gpt.InvalidRequest);

        string prompt = generatePromts.GenerateMerchant(merchantRequest);

        ResponseSchema schema = AiSchemaBuilder.BuildSchemaForMerchant();

        return await GenerateCharacterAsync<MerchantShop>(prompt, schema);
    }

    private async Task<Result<T>> GenerateCharacterAsync<T>(string prompt, ResponseSchema schema)
    where T : BaseCharacter
    {
         var aiResponse = await aiService.SendRequestToGeminiAsync<T>(prompt, schema);

        if (!aiResponse.IsSuccess) return Result<T>.Failure(aiResponse.Error!);

        var character = aiResponse.Value!;

        character.ImagePath = GetAvatarFromDump(character.Class);

        MappingInventory(character);

        return Result<T>.Success(character);
    }

    private void MappingInventory(BaseCharacter baseCharacter)
    {
        if(baseCharacter.InventoryTags == null) return;

        foreach(var item in baseCharacter.InventoryTags)
        {
            InventoryGenerationRequest generationRequest = new InventoryGenerationRequest
            {
                ClassName = baseCharacter.Class,
                Rarity = item.Rarity,
                Type = item.Type
            };

            var generatedItem = itemService.GetItemFromLocalDump(generationRequest);
            if (generatedItem.IsSuccess)
            {
                baseCharacter.InventoryDto.Add(generatedItem.Value!.DeepClone());
            }
        }
    }

    private string? GetAvatarFromDump(string? npcClass)
    {
        if (string.IsNullOrWhiteSpace(npcClass))
        {
            return Path.Combine(_baseAvatarUrlPath, "default_npc.png"); 
        };

        string fileName = $"{npcClass.ToLower()}.png";

        string fullPath = Path.Combine(_baseAvatarUrlPath, fileName);

        return fullPath;
    }
}