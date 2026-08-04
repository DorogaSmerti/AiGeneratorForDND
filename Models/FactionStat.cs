using static StoryTracker.Models.DomainErrors;
namespace StoryTracker.Models;

public class FactionStat
{
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? Goal { get; set; } = string.Empty;
    public string? Motivation { get; set; } = string.Empty;
    public string? RelationshipToPlayer { get; set; } = string.Empty;
    public int Reputation { get; set; }
    public string? Headquarters { get; set; } = string.Empty;
    public FactionLeader? Leader { get; set; }
}

public class FactionLeader
{
    public string? Name { get; set; } = string.Empty;
    public string? Race { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int? ChallengeRating { get; set; } = null;
    public string? ClassOrProfession { get; set; } = string.Empty;
    
    public string? Alignment { get; set; } = string.Empty;
}