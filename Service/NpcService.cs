using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class NpcService : INpcService
{
    private readonly HttpClient _httpClient;
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
        _itemService = itemService;
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

        var aiResponse = await SendRequestToGeminiAsync<BaseCharacter>(prompt, schema);

        if(!aiResponse.IsSuccess) return Result<BaseCharacter>.Failure(aiResponse.Error!);

        var npcStat = aiResponse.Value!;

        npcStat.ImagePath = GetAvatarFromDump(npcStat.Class);

        if (string.IsNullOrWhiteSpace(npcStat.ImagePath))
        {
            npcStat.ImagePath = Path.Combine(_baseAvatarUrlPath, "default_npc.png"); 
        }

        await MappingInventoryAsync(npcStat);

        await _npcExportService.ExportToFvttJsonAsync(npcStat, "База");

        return Result<BaseCharacter>.Success(npcStat);
    }

    public async Task<Result<MerchantShop>> GenerateMerchantAsync(MerchantRequest merchantRequest)
    {
        if (merchantRequest == null) return Result<MerchantShop>.Failure(DomainErrors.Gpt.InvalidRequest);

        string prompt = _generatePromts.GenerateMerchant(merchantRequest);

        ResponseSchema schema = AiSchemaBuilder.BuildSchemaForMerchant();

        var aiResponse = await SendRequestToGeminiAsync<MerchantShop>(prompt, schema);

        if (!aiResponse.IsSuccess) return Result<MerchantShop>.Failure(aiResponse.Error!);

        var merchant = aiResponse.Value;

        merchant.ImagePath = GetAvatarFromDump(merchant.Class);

        if (string.IsNullOrWhiteSpace(merchant.ImagePath))
        {
            merchant.ImagePath = Path.Combine(_baseAvatarUrlPath, "default_npc.png");
        }

        await MappingInventoryAsync(merchant);

        await _npcExportService.ExportToFvttJsonAsync(merchant, "Магазин");

        return Result<MerchantShop>.Success(merchant);
    }

    private async Task<Result<T>> SendRequestToGeminiAsync<T>(string prompt, ResponseSchema schema)
    where T : class
    {
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite:generateContent?key={_apiKey}";

        AiRequest aiRequest = new AiRequest
        {
            Contents = new List<Content>
            {
                new Content
                {
                   Parts = new List<Part>
                   {
                       new Part { Text = prompt}
                   } 
                }
            },
            GenerationConfig = new GenerationConfig
            {
                ResponseSchema = schema
            }
        };

        HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync(url, aiRequest);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var errorContent = await responseMessage.Content.ReadAsStringAsync();
            _logger.LogError("Error in GEMINI request {StatusCode}. Details: {ErrorContent}", 
                responseMessage.StatusCode, 
                errorContent);
            return Result<T>.Failure(DomainErrors.Gpt.ApiError);
        }

        var jsonDocument = await responseMessage.Content.ReadFromJsonAsync<JsonNode>();

        string? aiReply = (string?)jsonDocument?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"];;

        if (string.IsNullOrWhiteSpace(aiReply))
        {
            _logger.LogError("Ai return null");
            return Result<T>.Failure(DomainErrors.Gpt.GenerationFailed);
        }

        T? result = JsonSerializer.Deserialize<T>(aiReply, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if(result == null)
        {
            _logger.LogError("Json Deserialize return null");
            return Result<T>.Failure(DomainErrors.Gpt.ParseError);
        }

        return Result<T>.Success(result);
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