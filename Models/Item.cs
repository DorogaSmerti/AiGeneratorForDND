using StoryTracker.Models.Foundry;

namespace StoryTracker.Models;

public class Item
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Price? Price { get; set; }
    public string? Type { get; set; }
    public string? Rarity { get; set; } = "Обычный";
    public double? Weight { get; set; }
    public List<string>? Categories { get; set; } = new();
    public List<string>? Properties { get; set; } = new();
}

public class Weapon : Item
{
    public string? DamageDice { get; set; }
    public string? DamageType { get; set; }
    public TwoHanded? TwoHanded { get; set; }
    public ThrowRange? ThrowRange { get; set; }
    public RangeDto? Range { get; set; }
    public string? Ammunition { get; set; }
}

public class ThrowRange
{
    public int? Normal { get; set; }
    public int? Long { get; set; }
}

public class TwoHanded
{
    public string? DamageDice { get; set; }
    public string? DamageType { get; set; }
}

public class Armor : Item
{
    public int? ArmorClass { get; set; }
}

public class OtherItem : Item
{
    
}

public class Price
{
    public double? Gold { get; set; }
    public double? Silver { get; set; }
    public double? Copper { get; set; }
}