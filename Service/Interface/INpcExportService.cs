using StoryTracker.Models;

namespace StoryTracker.Service;

public interface INpcExportService
{
    Task<string> ExportToFvttJsonAsync(NpcStat npcJson);
}