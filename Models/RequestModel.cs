namespace StoryTracker.Models;

public class NpcRequest
{
    public string? Name { get; set; } = string.Empty;
    public string? Race { get; set; } = string.Empty;
    public int? ChallengeRating {get;set;} = null;
    public string? ClassOrProfession { get; set; } = string.Empty;
    public string? Alignment { get; set; } = string.Empty;
    public string? UserWishes { get; set; } = string.Empty;
}

public class MerchantRequest
{
    public string? MerchantName { get; set; } = string.Empty;
    public string? ShopType { get; set; } = string.Empty;
    public string? Wealth { get; set; } = string.Empty;
    public string? UserWishes { get; set; } = string.Empty;
}

public class FactionRequest
{
    public string? Name { get; set; } = string.Empty;
    public string? Type { get; set; } = string.Empty;
    public string? Reputation { get; set; } = string.Empty;
    public string? UserWishes { get; set; } = string.Empty;
}
