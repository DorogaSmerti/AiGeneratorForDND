namespace StoryTracker.Models;

public class AiRequest
{
    public List<Content> Contents { get; set; } = new();
    public GenerationConfig GenerationConfig { get; set; } = new();
}

public class Content
{
    public List<Part> Parts { get; set; } = new();
}

public class Part
{
    public string Text { get; set; }
}

public class GenerationConfig
{
    public string ResponseMimeType { get; set; } = "application/json";
    public ResponseSchema ResponseSchema { get; set; } = new();
}

public class ResponseSchema
{
    public string Type { get; set; } = "OBJECT";
    public Dictionary<string, SchemaProperty> Properties { get; set; } = new();
    public List<string> Required { get; set; } = new();
}

public class SchemaProperty
{
    public string Type { get; set; } = "STRING";
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, SchemaProperty> Properties { get; set; } = new();
    public SchemaProperty? Items { get; set; }
}