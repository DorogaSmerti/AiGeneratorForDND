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

    public NpcService(HttpClient httpClient, IItemService itemService, IGeneratePromts generatePromts, IConfiguration configuration, INpcExportService npcExportService)
    {
        _httpClient = httpClient;
        _itemService = itemService;
        _generatePromts = generatePromts;
        _configuration = configuration;
        _apiKey = _configuration["GeminiApi:ApiKey"] ?? throw new ArgumentNullException("GeminiApi:ApiKey configuration is missing.");
        _npcExportService = npcExportService;
    }

    public async Task<NpcStat> GenerateNpcAsync(NpcRequest npc)
    {
        if (npc == null) return null;

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

        NpcStat? npcStat = JsonSerializer.Deserialize<NpcStat>(aiReply, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        foreach(var item in npcStat.InventoryTags)
        {
            InventoryGenerationRequest generationRequest = new InventoryGenerationRequest
            {
                ClassName = npcStat.Class,
                ChallengeRating = 12,
                Rarity = item.Rarity,
                Type = item.Type
            };

            var generatedItem = await _itemService.GetItemFromLocalDump(generationRequest);
            
            if (generatedItem != null)
            {
                npcStat.InventoryDto.Add(generatedItem);
            }
        }

        await _npcExportService.ExportToFvttJsonAsync(npcStat);

        return npcStat;
    }
}