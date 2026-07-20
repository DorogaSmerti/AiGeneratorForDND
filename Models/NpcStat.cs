using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace StoryTracker.Models;

public class BaseCharacter
{
    [JsonPropertyOrder(-4)]
    public string? Name { get; set; } = string.Empty;
    [JsonPropertyOrder(-3)]
    public string? Race { get; set; } = string.Empty;
    [JsonPropertyOrder(-2)]
    public string? Class { get; set; } = string.Empty;
    [JsonPropertyOrder(-1)]
    public string? Description { get; set; } = string.Empty;
    [JsonPropertyOrder(0)]
    public string? ImagePath { get; set; } = string.Empty;
    public int ChallengeRating {get;set;}
    public string? Alignment {get;set;}
    public string? HookOrSecret {get;set;}
    public int Strength{get;set;}
    public int Dexterity{get;set;}
    public int Constitution{get;set;}
    public int Intelligence{get;set;}
    public int Wisdom{get;set;}
    public int Charisma { get; set; }
    public Biography? Biography{get;set;}
    public Journal? Journal{get;set;}
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<InventoryTags> InventoryTags { get; set; } = new();
    [JsonPropertyName("equipment")]
    public JsonArray InventoryDto { get; set; } = new();
}

public class MerchantShop : BaseCharacter
{
    public string ShopName { get; set; } = string.Empty;
    public string ShopDescription { get; set; } = string.Empty;
    public string MerchantDescription { get; set; } = string.Empty;
    public double PriceModifier { get; set; } = 1.0;
}

public class InventoryGenerationRequest
{
    public string? ClassName { get; set; } = string.Empty;
    public string? Type { get; set; } = string.Empty;
    public string? Rarity { get; set; } = string.Empty;
}

public class InventoryTags
{
    public string? Type {get;set;}
    public string? Rarity {get;set;}
}