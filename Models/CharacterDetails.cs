namespace StoryTracker.Models;

public class Biography
{
    public string? Gender { get; set; }
    public string? Age { get; set; }
    public string? Height { get; set; }
    public string? Weight { get; set; }
    public string? Eyes { get; set; }
    public string? Skin { get; set; }
    public string? Hair { get; set; }
    public string? Faith { get; set; }
    public string? Appearance { get; set; }
    public string? Background { get; set; }
}

public class Journal
{
    public string? PersonalOfInterest { get; set; }
    public string? LocationOfInterest { get; set; }
    public string? Quests { get; set; }
    public string? Miscellaneous { get; set; }
    public string? JournalEntries { get; set; }
}