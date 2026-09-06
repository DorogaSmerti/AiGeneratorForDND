namespace StoryTracker.Models;

public class EmbeddingResponse
{
    public EmbeddingValues? Embedding { get; set; }
}

public class EmbeddingValues
{
    public float[]? Values { get; set; }
}