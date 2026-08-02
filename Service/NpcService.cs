using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class NpcService : INpcService
{
    private readonly IAiService _aiService;
    private readonly ILogger<NpcService> _logger;
    private readonly IGeneratePromts _generatePromts;
    private readonly IItemService _itemService;
    private readonly IConfiguration _configuration;
    private readonly string _baseAvatarUrlPath;

    public NpcService(IAiService aiService, IItemService itemService, IGeneratePromts generatePromts, IConfiguration configuration, ILogger<NpcService> logger)
    {
        _aiService = aiService;
        _logger = logger;
        _itemService = itemService;
        _generatePromts = generatePromts;
        _configuration = configuration;
        _baseAvatarUrlPath = _configuration["AvatarSettings:AvatarPath"] ?? throw new ArgumentNullException("UrlPath configuration is missing.");
    }

    public async Task<Result<BaseCharacter>> GenerateNpcAsync(NpcRequest npcRequest)
    {
        if (npcRequest == null) return Result<BaseCharacter>.Failure(DomainErrors.Gpt.InvalidRequest);

        var prompt = _generatePromts.GenerateNpc(npcRequest);

        var schema = AiSchemaBuilder.BuildSchemaForNpc();

        var aiResponse = await _aiService.SendRequestToGeminiAsync<BaseCharacter>(prompt, schema);

        if(!aiResponse.IsSuccess) return Result<BaseCharacter>.Failure(aiResponse.Error!);

        var npcStat = aiResponse.Value!;

        npcStat.ImagePath = GetAvatarFromDump(npcStat.Class);

        if (string.IsNullOrWhiteSpace(npcStat.ImagePath))
        {
            npcStat.ImagePath = Path.Combine(_baseAvatarUrlPath, "default_npc.png"); 
        }

        await MappingInventoryAsync(npcStat);

        return Result<BaseCharacter>.Success(npcStat);
    }

    public async Task<Result<MerchantShop>> GenerateMerchantAsync(MerchantRequest merchantRequest)
    {
        if (merchantRequest == null) return Result<MerchantShop>.Failure(DomainErrors.Gpt.InvalidRequest);

        string prompt = _generatePromts.GenerateMerchant(merchantRequest);

        ResponseSchema schema = AiSchemaBuilder.BuildSchemaForMerchant();

        var aiResponse = await _aiService.SendRequestToGeminiAsync<MerchantShop>(prompt, schema);

        if (!aiResponse.IsSuccess) return Result<MerchantShop>.Failure(aiResponse.Error!);

        var merchant = aiResponse.Value;

        merchant.ImagePath = GetAvatarFromDump(merchant.Class);

        if (string.IsNullOrWhiteSpace(merchant.ImagePath))
        {
            merchant.ImagePath = Path.Combine(_baseAvatarUrlPath, "default_npc.png");
        }

        await MappingInventoryAsync(merchant);

        return Result<MerchantShop>.Success(merchant);
    }

    private async Task MappingInventoryAsync(BaseCharacter baseCharacter)
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

            var generatedItem = await _itemService.GetItemFromLocalDump(generationRequest);
            if (generatedItem.IsSuccess)
            {
                baseCharacter.InventoryDto.Add(generatedItem.Value!.DeepClone());
            }
        }
    }

    private string? GetAvatarFromDump(string? npcClass)
    {
        if(string.IsNullOrWhiteSpace(npcClass)) return null;

        string fileName = $"{npcClass.ToLower()}.png";

        string fullPath = Path.Combine(_baseAvatarUrlPath, fileName);

        return fullPath;
    }
}