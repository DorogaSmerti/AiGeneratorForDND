using System.Text.Json;
using System.Text.Json.Nodes;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class VectorService : IVectorService
{
    private readonly IAiService _aiService;
    private readonly IItemDataStorage _itemDataStorage;
    private const string CachePath = "LocalDump/vector_cache.json";

    public VectorService(IAiService aiService, IItemDataStorage itemDataStorage)
    {
        _aiService = aiService;
        _itemDataStorage = itemDataStorage;
    }

    public async Task<Result<int>> BuildDataBaseVectorAsync()
    {
        Dictionary<string, float[]> vectorDump = await LoadCacheAsync();
        List<JsonNode> localDump = _itemDataStorage.GetItems();
        int addedCount = 0;

        // take from dump the items is valid and not exists in cache
        var itemToProcess = localDump.Where(
            item =>
            !string.IsNullOrWhiteSpace((string?)item["_id"])
            && !string.IsNullOrWhiteSpace((string?)item["name"])
            && !vectorDump.ContainsKey((string?)item["_id"]!))
            .ToList();

        foreach (var item in itemToProcess)
        {
            string? _id = (string?)item["_id"];
            string? name = (string?)item["name"];

            var result = await _aiService.GenerateEmbeddingAsync(name!);

            if (!result.IsSuccess)
            {
                if (result.Error?.Code == "429")
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }

                continue;
            }

            vectorDump[_id!] = result.Value!;
            addedCount++;

            if (addedCount == 100)
            {
                await SaveCacheAsync(vectorDump);
                await Task.Delay(TimeSpan.FromMinutes(1));
                addedCount = 0;
            }
        }

        string json = JsonSerializer.Serialize(vectorDump);
        await File.WriteAllTextAsync(CachePath, json);

        return Result<int>.Success(vectorDump.Count);
    }
    private async Task<Dictionary<string, float[]>> LoadCacheAsync()
    {
        // Load cache from local dump file if exists
        if (!File.Exists(CachePath)) return new();

        string existJson = await File.ReadAllTextAsync(CachePath);
        return JsonSerializer.Deserialize<Dictionary<string, float[]>>(existJson) ?? new();
    }

    private async Task SaveCacheAsync(Dictionary<string, float[]> vectorDump)
    {
        //Save cache to local dump file
        string tempJson = JsonSerializer.Serialize(vectorDump);
        await File.WriteAllTextAsync(CachePath, tempJson);
    }
}
