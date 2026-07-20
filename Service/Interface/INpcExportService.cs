using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface INpcExportService
{
    Task<string> ExportToFvttJsonAsync(BaseCharacter npcJson, string templateName);
}