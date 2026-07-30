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
    public BaseCharacter? Leader { get; set; }
}