using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace StoryTracker.Models;

public class NpcStat
{
    public string Name{get;set;}
    public string Race {get;set;}
    public string Description {get;set;}
    public int ChallengeRating {get;set;}
    public string ImageUrl {get;set;}
    public string Alignment {get;set;}
    public string HookOrSecret {get;set;}
    public string Class {get;set;}
    public int Strength{get;set;}
    public int Dexterity{get;set;}
    public int Constitution{get;set;}
    public int Intelligence{get;set;}
    public int Wisdom{get;set;}
    public int Charisma{get;set;}
    public Biography Biography{get;set;}
    public Journal Journal{get;set;}
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<InventoryTags> InventoryTags{get;set;} = new();
    [JsonPropertyName("equipment")]
    public JsonArray InventoryDto {get; set; } = new();
}

    public class InventoryGenerationRequest
{
    public string? ClassName {get;set;} = string.Empty;
    public double? ChallengeRating {get;set;}
    public string? Type {get;set;}
    public string? Rarity {get;set;}
}

    public class InventoryTags
{
    public string Type {get;set;}
    public string Rarity {get;set;}
}