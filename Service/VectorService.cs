using System.Text.Json;
using System.Text.Json.Nodes;
using StoryTracker.Models;
using StoryTracker.Service.Interface;
using StoryTracker.Utils;

namespace StoryTracker.Service;

public class VectorService : IVectorService
{
    private readonly IAiService _aiService;
    private readonly IItemDataStorage _itemDataStorage;
    private readonly IItemService _itemService;
    private readonly ILogger<VectorService> _logger;
    private const string CachePath = "LocalDump/vector_cache.json";

    public VectorService(IAiService aiService, IItemDataStorage itemDataStorage, IItemService itemService, ILogger<VectorService> logger)
    {
        _aiService = aiService;
        _itemDataStorage = itemDataStorage;
        _itemService = itemService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<JsonNode>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query)) return Result<JsonNode>.Failure(DomainErrors.Vector.EmptyQuery);

        // Load cache from vector_cache.json file
        var vectorDump = await LoadCacheAsync();

        if (vectorDump.Count == 0) return Result<JsonNode>.Failure(DomainErrors.Vector.VectorCacheNotFound);

        // Generate embedding for the query
        var queryResult = await _aiService.GenerateEmbeddingAsync(query, cancellationToken);

        if (!queryResult.IsSuccess) return Result<JsonNode>.Failure(queryResult.Error!);

        // Find the most similar vector using cosine similarity
        var result = vectorDump.Select(pair =>
        new
        {
            Id = pair.Key,
            Similarity = VectorMath.CosineSimilarity(queryResult.Value!, pair.Value)
        }).OrderByDescending(x => x.Similarity)
        .FirstOrDefault();

        if (result == null) return Result<JsonNode>.Failure(DomainErrors.Vector.NoVectorFound);

        // Get item from the database
        var item = _itemService.GetItemById(result.Id);

        if (!item.IsSuccess) return Result<JsonNode>.Failure(item.Error!);

        return Result<JsonNode>.Success(item.Value!);
    }

    /// <inheritdoc />
    public async Task<Result<int>> BuildVectorDataBaseAsync(CancellationToken cancellationToken = default)
    {
        // Load existing cache from disk
        Dictionary<string, float[]> vectorDump = await LoadCacheAsync();
        List<JsonNode> localDump = _itemDataStorage.GetItems();
        int addedCount = 0;

        // Filter valid items that have not been indexed yet
        var itemToProcess = localDump.Where(
            item =>

            !string.IsNullOrWhiteSpace((string?)item["_id"])
            &&
            !string.IsNullOrWhiteSpace((string?)item["name"])
            &&
            !vectorDump.ContainsKey((string)item["_id"]!)
            )
            .ToList();

        foreach (var item in itemToProcess)
        {

            if (cancellationToken.IsCancellationRequested) break;

            string? _id = (string?)item["_id"];
            string? name = (string?)item["name"];

            // Generate embedding vector via Gemini API
            var result = await _aiService.GenerateEmbeddingAsync(name!, cancellationToken);

            if (!result.IsSuccess)
            {
                // Pause for 1 minute if rate limit is hit (HTTP 429)
                if (result.Error?.Code == DomainErrors.Gpt.ApiError.Code)
                {
                    _logger.LogWarning("Rate limit reached (429). Pausing embedding generation for 1 minute...");
                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                }

                continue;
            }

            vectorDump[_id!] = result.Value!;
            addedCount++;

            // Save intermediate cache every 100 items to avoid losing progress and delays from API limits
            if (addedCount == 100)
            {
                await SaveCacheAsync(vectorDump);
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Saved intermediate cache. Current vector size: {Count}", vectorDump.Count);
                    _logger.LogInformation("Delay for 1 minute cause limit");
                }
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                addedCount = 0;
            }
        }
        // Save final cache to disk
        await SaveCacheAsync(vectorDump);

        return Result<int>.Success(vectorDump.Count);
    }
    private static async Task<Dictionary<string, float[]>> LoadCacheAsync()
    {
        // Load cache from local dump file if exists
        if (!File.Exists(CachePath)) return new();

        string existJson = await File.ReadAllTextAsync(CachePath);
        return JsonSerializer.Deserialize<Dictionary<string, float[]>>(existJson) ?? new();
    }

    private static async Task SaveCacheAsync(Dictionary<string, float[]> vectorDump)
    {
        //Save cache to local dump file
        string tempJson = JsonSerializer.Serialize(vectorDump);
        await File.WriteAllTextAsync(CachePath, tempJson);
    }
}
