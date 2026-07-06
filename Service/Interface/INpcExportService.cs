using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface INpcExportService
{
    Task<string> ExportToFvttJsonAsync(NpcStat npcJson);
}