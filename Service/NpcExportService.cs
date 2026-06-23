using System.Text;
using StoryTracker.Models;

namespace StoryTracker.Service;

public class NpcExportService : INpcExportService
{

    public NpcExportService()
    {
    }

    public async Task<string> ExportToFvttJsonAsync(NpcStat npcJson)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "База.json");
        string template = await File.ReadAllTextAsync(path);

        var sb = new StringBuilder(template);

            sb.Replace("{{Name}}", npcJson.Name ?? "");
            sb.Replace("{{Race}}", npcJson.Race ?? "");
            sb.Replace("{{Alignment}}", npcJson.Alignment ?? "");
            sb.Replace("{{HookOrSecret}}", npcJson.HookOrSecret ?? "");

            // --- Блок Biography ---
            sb.Replace("{{Gender}}", npcJson.Biography?.Gender ?? "");
            sb.Replace("{{Age}}", npcJson.Biography?.Age ?? "");
            sb.Replace("{{Height}}", npcJson.Biography?.Height ?? "");
            sb.Replace("{{Weight}}", npcJson.Biography?.Weight ?? "");
            sb.Replace("{{Eyes}}", npcJson.Biography?.Eyes ?? "");
            sb.Replace("{{Skin}}", npcJson.Biography?.Skin ?? "");
            sb.Replace("{{Hair}}", npcJson.Biography?.Hair ?? "");
            sb.Replace("{{Faith}}", npcJson.Biography?.Faith ?? "");
            sb.Replace("{{Appearance}}", npcJson.Biography?.Appearance ?? "");
            sb.Replace("{{Background}}", npcJson.Biography?.Background ?? "");

            // --- Блок Journal ---
            sb.Replace("{{PersonalOfInterest}}", npcJson.Journal?.PersonalOfInterest ?? "");
            sb.Replace("{{LocationOfInterest}}", npcJson.Journal?.LocationOfInterest ?? "");
            sb.Replace("{{Quests}}", npcJson.Journal?.Quests ?? "");
            sb.Replace("{{Miscellaneous}}", npcJson.Journal?.Miscellaneous ?? "");
            sb.Replace("{{JournalEntries}}", npcJson.Journal?.JournalEntries ?? "");

            sb.Replace("{{Items}}", npcJson.InventoryDto.ToString() ?? "");

            return sb.ToString();
    }
}