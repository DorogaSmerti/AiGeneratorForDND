using System.Text;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class NpcExportService : INpcExportService
{

    public NpcExportService()
    {
    }

    public async Task<string> ExportToFvttJsonAsync(BaseCharacter npcJson, string templateName)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", $"{templateName}.json");
        string template = await File.ReadAllTextAsync(path);

        var sb = new StringBuilder(template);

        ReplaceCommonPlaceholders(sb, npcJson);

        if (npcJson is MerchantShop merchantShop)
        {
            ReplaceMerchantPlaceholders(sb, merchantShop);
        }

        string fvttJson = sb.ToString();

        await ExportInJsonFile(fvttJson, npcJson);

        return fvttJson;
    }

    private static void ReplaceCommonPlaceholders(StringBuilder sb, BaseCharacter npcJson)
    {
        sb.Replace("{{Name}}", npcJson.Name ?? "");
        sb.Replace("{{Race}}", npcJson.Race ?? "");
        sb.Replace("{{Alignment}}", npcJson.Alignment ?? "");
        sb.Replace("{{HookOrSecret}}", npcJson.HookOrSecret ?? "");
        sb.Replace("{{Images}}", npcJson.ImagePath ?? "");
        sb.Replace("\"{{ChallengeRating}}\"", npcJson.ChallengeRating.ToString());

        // --- Biography ---
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

        // --- Journal ---
        sb.Replace("{{PersonalOfInterest}}", npcJson.Journal?.PersonalOfInterest ?? "");
        sb.Replace("{{LocationOfInterest}}", npcJson.Journal?.LocationOfInterest ?? "");
        sb.Replace("{{Quests}}", npcJson.Journal?.Quests ?? "");
        sb.Replace("{{Miscellaneous}}", npcJson.Journal?.Miscellaneous ?? "");
        sb.Replace("{{JournalEntries}}", npcJson.Journal?.JournalEntries ?? "");

        sb.Replace("\"{{Items}}\"", npcJson.InventoryDto.ToString() ?? "[]");
    }
    
    private static void ReplaceMerchantPlaceholders(StringBuilder sb, MerchantShop merchantShop)
    {
        sb.Replace("{{ShopName}}", merchantShop.ShopName ?? "");
        sb.Replace("{{ShopDescription}}", merchantShop.ShopDescription ?? "");
        sb.Replace("{{MerchantDescription}}", merchantShop.MerchantDescription ?? "");
        sb.Replace("\"{{PriceModifier}}\"", merchantShop.PriceModifier.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    private async Task ExportInJsonFile(string fvttJson, BaseCharacter baseCharacter)
    {
        var exportDir = Path.Combine(Directory.GetCurrentDirectory(), "Export");

        if (!Directory.Exists(exportDir))
        {
            Directory.CreateDirectory(exportDir);
        }

        string npcName = !string.IsNullOrWhiteSpace(baseCharacter.Name) ? baseCharacter.Name : "Generated_NPC";
        string fileName = $"{npcName}.json";
        string fullPath = Path.Combine(exportDir, fileName);

        await File.WriteAllTextAsync(fullPath, fvttJson);
    }
}