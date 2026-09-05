using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class VectorService(IAiService _aiService, IItemDataStorage _itemDataStorage) : IVectorService
{
    public async Task<Result<int>> BuildDataBaseVectorAsync(int limit)
    {
        Dictionary<string, float[]> vectorDump = new();
        List<JsonNode> localDump = _itemDataStorage.GetItems();
        int addedCount = 0;

        if (limit <= 0) limit = 50;

        if (File.Exists("LocalDump/vector_cache.json"))
        {
            string existJson = await File.ReadAllTextAsync("LocalDump/vector_cache.json");
            vectorDump = JsonSerializer.Deserialize<Dictionary<string, float[]>>(existJson) ?? new();
        }

        foreach(var item in localDump)
        {

            if (addedCount >= limit) break;

            string? _id = (string?)item["_id"];
            string? name = (string?)item["name"];

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(_id)) continue;

            if (vectorDump.ContainsKey(_id)) continue;

            var result = await _aiService.GenerateEmbeddingAsync(name);

            if (!result.IsSuccess) continue;

            vectorDump[_id] = result.Value!;
            addedCount++;
        }

        string json = JsonSerializer.Serialize(vectorDump);
        await File.WriteAllTextAsync("LocalDump/vector_cache.json", json);

        return Result<int>.Success(vectorDump.Count);
    }
}
