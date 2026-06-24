namespace StoryTracker.Models;

public class NpcRequest
{
    public string? Name { get; set; } = string.Empty;
    public string? Race { get; set; } = string.Empty;
    public int? ChallengeRating {get;set;}
    public string? ClassOrProfession { get; set; } = string.Empty;
    public string? Alignment { get; set; } = string.Empty;
    public string? UserWishes { get; set; } = string.Empty;
}