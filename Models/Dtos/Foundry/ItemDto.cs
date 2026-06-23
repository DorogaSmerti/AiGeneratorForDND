using System.Text.Json.Serialization;

namespace StoryTracker.Models.Foundry;

public class ItemDtoFoundry
{
    [JsonPropertyName("_id")]
    public string? Id {get;set;}
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Img {get;set;}
    public System? System { get; set; }
    [JsonExtensionData]
    public Dictionary<string, object>? ExtensionData{get;set;} = new();

}

public class System
{
    public Description? Description { get; set; }
    public int? Quaintity { get; set; }
    public double? Weight { get; set; }
    public foundryPrice? Price { get; set; }
    public int? Attunement {get;set;}
    public bool? Equipped {get;set;}
    public string? Rarity {get;set;}
    public bool? Identified {get;set;}
    public Activation? Activation {get;set;}
    public Duration? Duration {get;set;}
    public Target? Target {get;set;}
    public RangeDto? Range {get;set;}
    public string? ActionType { get; set; }
    public string? WeaponType { get; set; }
    public string? BaseItem { get; set; }
    public DamageDto? Damage{get;set;}
}

public class DamageDto
{
    public string[][]? Parts {get;set;}
    public string? Versatile { get; set; }
}

public class Target
{
    public int? Value { get; set; }
    public string? Units { get; set; }
    public string? Type { get; set; }
}

public class RangeDto
{
    public int? Value { get; set; }
    public int? Long { get; set; }
    public string? Units { get; set; }
}

public class Duration
{
    public string? Value {get;set;}
    public string? Units {get;set;}
}

public class Description
{
    public string? Value { get; set; }
    public string? Chat { get; set; }
    public string? Unidentified { get; set; }
}

public class foundryPrice
{
    public double? Value { get; set; }
    public string? Denomination { get; set; }
}

public class Activation
{
    public string? Type {get;set;}
    public int? Cost {get;set;}
    public string? Condition {get;set;}
}