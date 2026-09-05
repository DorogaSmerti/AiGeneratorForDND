using System.Text.Json;
using System.Text.Json.Nodes;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AiService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;

    public AiService(HttpClient httpClient, ILogger<AiService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        _apiKey = _configuration["GeminiApi:ApiKey"] ?? throw new ArgumentNullException("GeminiApi:ApiKey configuration is missing.");
    }

    public async Task<Result<T>> SendRequestToGeminiAsync<T>(string prompt, ResponseSchema schema)
    where T : class
    {
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite:generateContent?key={_apiKey}";

        AiRequest aiRequest = new AiRequest
        {
            Contents = new List<Content>
            {
                new Content
                {
                   Parts = new List<Part>
                   {
                       new Part { Text = prompt}
                   }
                }
            },
            GenerationConfig = new GenerationConfig
            {
                ResponseSchema = schema
            }
        };

        HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync(url, aiRequest);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var errorContent = await responseMessage.Content.ReadAsStringAsync();
            _logger.LogError("Error in GEMINI request {StatusCode}. Details: {ErrorContent}",
                responseMessage.StatusCode,
                errorContent);
            return Result<T>.Failure(DomainErrors.Gpt.ApiError);
        }

        var jsonDocument = await responseMessage.Content.ReadFromJsonAsync<JsonNode>();

        string? aiReply = (string?)jsonDocument?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]; ;

        if (string.IsNullOrWhiteSpace(aiReply))
        {
            _logger.LogError("Ai return null");
            return Result<T>.Failure(DomainErrors.Gpt.GenerationFailed);
        }

        T? result = JsonSerializer.Deserialize<T>(aiReply, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null)
        {
            _logger.LogError("Json Deserialize return null");
            return Result<T>.Failure(DomainErrors.Gpt.ParseError);
        }

        return Result<T>.Success(result);
    }

    public async Task<Result<float[]>> GenerateEmbeddingAsync(string text)
    {
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-embedding-2:embedContent?key={_apiKey}";

        var aiRequest = new
        {
            model = "gemini-embedding-2",
            content = new Content
            {
                Parts = new List<Part>
                {
                    new Part {Text = text}
                }
            }
        };

        HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync(url, aiRequest);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var errorContent = await responseMessage.Content.ReadAsStringAsync();
            _logger.LogError("Error in GEMINI request {StatusCode}. Details: {ErrorContent}",
                responseMessage.StatusCode,
                errorContent);
            return Result<float[]>.Failure(DomainErrors.Gpt.ApiError);
        }

        EmbeddingResponse result = await responseMessage.Content.ReadFromJsonAsync<EmbeddingResponse>();

        if (result?.Embedding?.Values == null)
        {
            _logger.LogError("Failed to deserialize embedding values");
            return Result<float[]>.Failure(DomainErrors.Gpt.ParseError);
        }

        return Result<float[]>.Success(result.Embedding.Values);
    }
}