using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Routing;
using StoryTracker.Models;

namespace StoryTracker.Service;

public class NpcService : INpcService
{
    private readonly HttpClient _httpClient;
    private readonly IGeneratePromts _generatePromts;
    private readonly IItemService _itemService;
    private readonly IConfiguration _configuration;
    private readonly INpcExportService _npcExportService;
    private readonly string _apiKey;
    private readonly string _baseAvatarUrlPath;

    public NpcService(HttpClient httpClient, IItemService itemService, IGeneratePromts generatePromts, IConfiguration configuration, INpcExportService npcExportService)
    {
        _httpClient = httpClient;
        _itemService = itemService;
        _generatePromts = generatePromts;
        _configuration = configuration;
        _apiKey = _configuration["GeminiApi:ApiKey"] ?? throw new ArgumentNullException("GeminiApi:ApiKey configuration is missing.");
        _baseAvatarUrlPath = _configuration["AvatarSettings:AvatarPath"] ?? throw new ArgumentNullException("UrlPath configuration is missing.");
        _npcExportService = npcExportService;
    }

    public async Task<NpcStat?> GenerateNpcAsync(NpcRequest npc)
    {
        if (npc == null) return null;

        string? aiReply = await SendRequestToGeminiAsync(npc);

        if(string.IsNullOrWhiteSpace(aiReply)) return null;

        NpcStat? npcStat = JsonSerializer.Deserialize<NpcStat>(aiReply, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if(npcStat == null) return null;

        npcStat.ImagePath = GetAvatarFromDump(npcStat.Class);

        if (string.IsNullOrWhiteSpace(npcStat.ImagePath))
        {
            npcStat.ImagePath = Path.Combine(_baseAvatarUrlPath, "default_npc.png"); 
        }

        await MappingInventoryAsync(npcStat);

        await _npcExportService.ExportToFvttJsonAsync(npcStat);

        return npcStat;
    }

    private async Task<string?> SendRequestToGeminiAsync(NpcRequest npc)
    {
        var text = _generatePromts.GenerateNpc(npc);

        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite:generateContent?key={_apiKey}";

        AiRequest aiRequest = new AiRequest
        {
            Contents = new List<Content>
            {
                new Content
                {
                   Parts = new List<Part>
                   {
                       new Part { Text = text}
                   } 
                }
            },
            GenerationConfig = new GenerationConfig
            {
                ResponseSchema = AiSchemaBuilder.BuildSchema()
            }
        };

        HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync(url, aiRequest);

        if (!responseMessage.IsSuccessStatusCode)
        {
            Console.WriteLine($"{await responseMessage.Content.ReadAsStringAsync()}");
            return null;
        }

        var jsonDocument = await responseMessage.Content.ReadFromJsonAsync<JsonNode>();

        string? aiReply = (string?)jsonDocument?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"];;

        if (string.IsNullOrWhiteSpace(aiReply))
        {
            Console.WriteLine("ИИ вернул пустой текст или структура ответа изменилась.");
            return null;
        }

        return aiReply;
    }

    private async Task MappingInventoryAsync(NpcStat npcStat)
    {
        if(npcStat.InventoryTags == null) return;

        foreach(var item in npcStat.InventoryTags)
        {
            InventoryGenerationRequest generationRequest = new InventoryGenerationRequest
            {
                ClassName = npcStat.Class,
                ChallengeRating = npcStat.ChallengeRating,
                Rarity = item.Rarity,
                Type = item.Type
            };

            var generatedItem = await _itemService.GetItemFromLocalDump(generationRequest);
            
            if (generatedItem != null)
            {
                npcStat.InventoryDto.Add(generatedItem);
            }
        }
    }

    private string? GetAvatarFromDump(string npcClass)
    {
        if(string.IsNullOrWhiteSpace(npcClass)) return null;

        string fileName = $"{npcClass.ToLower()}.png";

        string fullPath = Path.Combine(_baseAvatarUrlPath, fileName);

        return fullPath;
    }
}