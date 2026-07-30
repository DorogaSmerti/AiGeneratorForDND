using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class FactionService : IFactionService
{
    private readonly IGeneratePromts _generatePromts;

    public FactionService(IGeneratePromts generatePromts)
    {
        _generatePromts = generatePromts;
    }

    public async Task<Result<FactionStat>> GenerateFactionAsync(FactionRequest factionRequest)
    {
        if (factionRequest == null) return Result<FactionStat>.Failure(DomainErrors.Faction.InvalidRequest);
    }
}