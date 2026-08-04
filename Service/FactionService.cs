using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class FactionService : IFactionService
{
    private readonly IGeneratePromts _generatePromts;
    private readonly IAiService _aiService;

    public FactionService(IGeneratePromts generatePromts, IAiService aiService)
    {
        _generatePromts = generatePromts;
        _aiService = aiService;
    }

    public async Task<Result<FactionStat>> GenerateFactionAsync(FactionRequest factionRequest)
    {
        if (factionRequest == null) return Result<FactionStat>.Failure(DomainErrors.Faction.InvalidRequest);

        string promt = _generatePromts.GenerateFaction(factionRequest);

        var schema = AiSchemaBuilder.BuildSchemaForFaction();

        var aiResponse = await _aiService.SendRequestToGeminiAsync<FactionStat>(promt, schema);

        if (!aiResponse.IsSuccess) return Result<FactionStat>.Failure(aiResponse.Error!);

        return Result<FactionStat>.Success(aiResponse.Value);
    }
}