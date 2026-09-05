using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface IVectorService
{
    Task<Result<int>> BuildDataBaseVectorAsync(int limit);
}