using StoryTracker.Models;
namespace StoryTracker.Service.Interface;

public interface IFactionService
{
    Task<Result<FactionStat>> GenerateFactionAsync(FactionRequest factionRequest);
}