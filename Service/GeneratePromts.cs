using System.Text;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

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
        userPrompt.AppendLine("5. In the 'InventoryTags' array, generate a list of 1-3 objects. Each object MUST strictly contain two fields: 'type' and 'rarity'. Allowed values for 'type' are: 'weapon', 'consumable', 'equipment', 'mgc', 'loot'. Allowed values for 'rarity' are: 'common', 'uncommon', 'rare', 'veryRare', 'legendary', 'artifact'. Choose these tags logically based on the NPC's class and level.");
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

    public string GenerateMerchant(MerchantRequest merchantRequest)
    {
        StringBuilder userPrompt = new StringBuilder();

        userPrompt.AppendLine("You are a helpful assistant for a game master in a tabletop roleplaying game. Your task is to generate a detailed description of a merchant and their shop based on the following attributes:");

        if (!string.IsNullOrEmpty(merchantRequest.MerchantName))
        {
            userPrompt.AppendLine($"Name: {merchantRequest.MerchantName}");
        }
        if (!string.IsNullOrEmpty(merchantRequest.ShopType))
        {
            userPrompt.AppendLine($"Shop Type: {merchantRequest.ShopType}");
        }
        if (!string.IsNullOrEmpty(merchantRequest.Wealth))
        {
            userPrompt.AppendLine($"Wealth: {merchantRequest.Wealth}");
        }
        if (!string.IsNullOrEmpty(merchantRequest.UserWishes))
        {
            userPrompt.AppendLine($"User Wishes: {merchantRequest.UserWishes}");
        }

        userPrompt.AppendLine(@"
        ### CRITICAL INSTRUCTIONS FOR AI:
        1. **PriceModifier**: Generate a double representing the shop's price multiplier (default is 1.0). 
        - A greedy merchant or remote/dangerous location might have a modifier of 1.1 to 1.5.
        - A generous merchant, regular merchant, or poor town might have a modifier of 0.8 to 1.0.
        2. **ShopName & ShopDescription**: Generate a thematic name and atmospheric description of the shop matching its type and wealth.
        3. **MerchantName & MerchantDescription**: If MerchantName was not provided, generate a fitting name. Provide a description of their appearance, race, behavior, and attitude towards customers.
        4. **InventoryTags**: Generate a list of item tags to populate the shop's stock.
        - The type/category of generated items must match the ShopType (e.g., Alchemist sells 'consumable'; Blacksmith sells 'weapon', 'equipment'; Magic Shop sells 'mgc', 'consumable').
        - Use only the allowed Type values: 'weapon', 'consumable', 'equipment', 'loot', 'mgc'.
        - Use only the allowed Rarity values: 'common', 'uncommon', 'rare', 'veryRare', 'legendary', 'artifact'.
        - The rarity of items must match the Wealth Level:
            - 'Poor' (village shop): 90% common, 10% uncommon. No rare/legendary items.
            - 'Medium' (town store): 70% common, 25% uncommon, 5% rare.
            - 'Rich' (capital city or magic academy): 40% common, 40% uncommon, 15% rare, 5% veryRare/legendary.");

        return userPrompt.ToString();
    }

    public string GenerateFaction(FactionRequest factionRequest)
    {
        var userPrompt = new StringBuilder();

        userPrompt.AppendLine("You are an expert Dungeon Master and Worldbuilder for Dungeons & Dragons 5e.");
        userPrompt.AppendLine("Create a detailed, atmospheric, and compelling Faction/Guild for a D&D 5e campaign based on the following user parameters:");

        if (!string.IsNullOrWhiteSpace(factionRequest.Name))
            userPrompt.AppendLine($"- Specified Name: {factionRequest.Name}");

        if (!string.IsNullOrWhiteSpace(factionRequest.Type))
            userPrompt.AppendLine($"- Faction Type/Category: {factionRequest.Type}");

        if (!string.IsNullOrWhiteSpace(factionRequest.Reputation))
            userPrompt.AppendLine($"- Reputation/Standing: {factionRequest.Reputation}");

        if (!string.IsNullOrWhiteSpace(factionRequest.UserWishes))
            userPrompt.AppendLine($"- Additional User Wishes: {factionRequest.UserWishes}");

        userPrompt.AppendLine("\nInstructions:");
        userPrompt.AppendLine("1. **Faction Name & Description**: Generate a memorable, thematic name if not provided. Describe the faction's history, ideology, and influence in the region.");
        userPrompt.AppendLine("2. **Goal & Motivation**: Define their ultimate objective and what drives them.");
        userPrompt.AppendLine("3. **RelationshipToPlayer**: Explain how they view adventuring parties and potential allies or enemies.");
        userPrompt.AppendLine("4. **Headquarters**: Describe their main base, guildhall, or secret hideout.");
        userPrompt.AppendLine("5. **Leader (Full NPC)**: Generate a complete D&D 5e character for the Faction Leader, including Name, Race, Class, Stats, Alignment, Description, HookOrSecret, and Biography.");

        return userPrompt.ToString();
    }
}