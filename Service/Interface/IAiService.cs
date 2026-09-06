using StoryTracker.Models;
namespace StoryTracker.Service.Interface;

public interface IAiService
{
    Task<Result<T>> SendRequestToGeminiAsync<T>(string prompt, ResponseSchema schema)
    where T : class;
    Task<Result<float[]>> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
}