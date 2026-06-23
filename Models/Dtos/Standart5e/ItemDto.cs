namespace StoryTracker.Models.Standart5e;
public class ItemDtoStandart5e
{
    public string? Name{get;set;}
    public Price? Price { get; set; }
    public double? Weight { get; set; }
    public string? Description { get; set; }
    public List<string>? Categories { get; set; } = new();
}

public class WeaponDto : ItemDtoStandart5e
{
    public Damage? Damage { get; set; }
    public TwoHanded? TwoHanded { get; set; }
    public ThrowRange? ThrowRange { get; set; }
    public Range? Range { get; set; }
    public string? Ammunition { get; set; }
}   

public class Damage
{
    public string? DamageDice { get; set; }
    public DamageType? DamageType { get; set; }
}

public class DamageType
{
    public string? index{get;set;}
    public string? Name { get; set; }
    public string? Url { get; set; }
}

public class Range
{
    public int? Normal { get; set; }
    public int? Long { get; set; }
}
