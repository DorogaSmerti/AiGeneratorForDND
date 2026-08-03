using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class NpcService : INpcService
{
    private readonly INpcEnrichmentService _npcEnrichmentService;
    private readonly IAiService _aiService;
    private readonly ILogger<NpcService> _logger;
    private readonly IGeneratePromts _generatePromts;
    private readonly IItemService _itemService;
    private readonly IConfiguration _configuration;
    private readonly INpcExportService _npcExportService;
    private readonly string _apiKey;
    private readonly string _baseAvatarUrlPath;

    public NpcService(HttpClient httpClient, IItemService itemService, IGeneratePromts generatePromts, IConfiguration configuration, INpcExportService npcExportService
    , ILogger<NpcService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _generatePromts = generatePromts;
        _configuration = configuration;
        _apiKey = _configuration["GeminiApi:ApiKey"] ?? throw new ArgumentNullException("GeminiApi:ApiKey configuration is missing.");
        _baseAvatarUrlPath = _configuration["AvatarSettings:AvatarPath"] ?? throw new ArgumentNullException("UrlPath configuration is missing.");
        _npcExportService = npcExportService;
    }

    public async Task<Result<BaseCharacter>> GenerateNpcAsync(NpcRequest npcRequest)
    {
        if (npcRequest == null) return Result<BaseCharacter>.Failure(DomainErrors.Gpt.InvalidRequest);

        var prompt = _generatePromts.GenerateNpc(npcRequest);

        var schema = AiSchemaBuilder.BuildSchemaForNpc();

        var aiResponse = await _aiService.SendRequestToGeminiAsync<BaseCharacter>(prompt, schema);

        if(!aiResponse.IsSuccess) return Result<BaseCharacter>.Failure(aiResponse.Error!);

        var npcStat = aiResponse.Value!;

        npcStat.ImagePath = _npcEnrichmentService.GetAvatarFromDump(npcStat.Class);

        await _npcEnrichmentService.MappingInventoryAsync(npcStat);

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

        merchant.ImagePath = _npcEnrichmentService.GetAvatarFromDump(merchant.Class);

        await _npcEnrichmentService.MappingInventoryAsync(merchant);

        return Result<MerchantShop>.Success(merchant);
    }

}