using System.Text;
using StoryTracker.Models;

namespace StoryTracker.Service;

public class GeneratePromts : IGeneratePromts
{

    public string GenerateNpc(NpcRequest npc)
    {
        StringBuilder userPrompt = new StringBuilder();
        userPrompt.AppendLine("You are a helpful assistant for a game master in a tabletop roleplaying game. Your task is to generate a detailed description of a non-player character (NPC) based on the following attributes:");

        if (!string.IsNullOrEmpty(npc.Name))
        {
            userPrompt.AppendLine($"Name: {npc.Name}");
        }
        if (!string.IsNullOrEmpty(npc.Race))
        {
            userPrompt.AppendLine($"Race: {npc.Race}");
        }
        if (!string.IsNullOrEmpty(npc.ClassOrProfession))
        {
            userPrompt.AppendLine($"Class/Profession: {npc.ClassOrProfession}");
        }
        if (npc.ChallengeRating != null)
        {
            userPrompt.AppendLine($"Challenge Rating: {npc.ChallengeRating}");
        }
        if (!string.IsNullOrEmpty(npc.Alignment))
        {
            userPrompt.AppendLine($"Alignment: {npc.Alignment}");
        }
        if (!string.IsNullOrEmpty(npc.UserWishes))
        {
            userPrompt.AppendLine($"User Wishes: {npc.UserWishes}");
        }

        userPrompt.AppendLine("\n### CRITICAL INSTRUCTIONS FOR AI:");
        userPrompt.AppendLine("1. STRICTLY ADHERE to all 'User Wishes' provided above. They have the highest priority. If a name, race, or profession is specified, DO NOT change it.");
        userPrompt.AppendLine("2. Since this is an NPC (not a player character), DO NOT generate standard starting class equipment.");
        userPrompt.AppendLine("3. Instead, invent 3-4 thematic personal items appropriate for their profession/social status (e.g., keys, a letter, specific tools, or a unique trinket) and place them in the 'Inventory' array.");
        userPrompt.AppendLine("4. Output MUST strictly match the provided JSON schema. Do not include any markdown formatting outside the JSON.");
        userPrompt.AppendLine("5. In the 'InventoryTags' array, generate a list of 1-3 objects. Each object MUST strictly contain two fields: 'Type' and 'Rarity'. Allowed values for 'Type' are: 'weapon', 'consumable', 'equipment', 'magic'. Allowed values for 'Rarity' are: 'common', 'uncommon', 'rare', 'very rare', 'legendary', 'artifact'. Choose these tags logically based on the NPC's class and level.");
        userPrompt.AppendLine($@" ### INVENTORY GENERATION RULES (Inventory Tags)
                                You must strictly generate the 'inventoryTags' array based on the character's danger level (Challenge Rating = {npc.ChallengeRating}). 
                                Apply STRICT limitations for the 'rarity' field depending on the current CR ({npc.ChallengeRating}):

                                - If CR is from 0 to 4: You are allowed to use ONLY ""common"" and ""uncommon"" (maximum 1 item). Strict ban on rare, veryRare, legendary.
                                - If CR is from 5 to 10: You are allowed to use ""common"", ""uncommon"", and ""rare"" (maximum 1-2 items). Ban on veryRare, legendary.
                                - If CR is from 11 to 16: You are allowed to use ""uncommon"", ""rare"", and ""veryRare"". Ban on legendary.
                                - If CR is from 17 to 30+: Any rarity types are allowed, including ""legendary"" (for bosses).

                                AI, remember: the inventory must fit the character's logic and background! A common guard cannot carry rare magical artifacts.");
        return userPrompt.ToString();
    }
}