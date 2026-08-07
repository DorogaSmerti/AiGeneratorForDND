using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class NpcService : INpcService
{
    private readonly INpcEnrichmentService _npcEnrichmentService;
    private readonly IAiService _aiService;
    private readonly ILogger<NpcService> _logger;
    private readonly IGeneratePromts _generatePromts;

    public NpcService(INpcEnrichmentService npcEnrichmentService, IAiService aiService, IGeneratePromts generatePromts, ILogger<NpcService> logger)
    {
        _npcEnrichmentService = npcEnrichmentService;
        _aiService = aiService;
        _logger = logger;
        _generatePromts = generatePromts;
    }

    public async Task<Result<BaseCharacter>> GenerateNpcAsync(NpcRequest npcRequest)
    {
        if (npcRequest == null) return Result<BaseCharacter>.Failure(DomainErrors.Gpt.InvalidRequest);

        string prompt = _generatePromts.GenerateNpc(npcRequest);

        ResponseSchema schema = AiSchemaBuilder.BuildSchemaForNpc();

        return await GenerateCharacterAsync<BaseCharacter>(prompt, schema);
    }

    public async Task<Result<MerchantShop>> GenerateMerchantAsync(MerchantRequest merchantRequest)
    {
        if (merchantRequest == null) return Result<MerchantShop>.Failure(DomainErrors.Gpt.InvalidRequest);

        string prompt = _generatePromts.GenerateMerchant(merchantRequest);

        ResponseSchema schema = AiSchemaBuilder.BuildSchemaForMerchant();

        return await GenerateCharacterAsync<MerchantShop>(prompt, schema);
    }

    private async Task<Result<T>> GenerateCharacterAsync<T>(string prompt, ResponseSchema schema)
    where T : BaseCharacter
    {
         var aiResponse = await _aiService.SendRequestToGeminiAsync<T>(prompt, schema);

        if (!aiResponse.IsSuccess) return Result<T>.Failure(aiResponse.Error!);

        var character = aiResponse.Value!;

        character.ImagePath = _npcEnrichmentService.GetAvatarFromDump(character.Class);

        await _npcEnrichmentService.MappingInventoryAsync(character);

        return Result<T>.Success(character);
    }

}