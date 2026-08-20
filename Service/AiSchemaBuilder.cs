using StoryTracker.Models;

namespace StoryTracker.Service;

public static class AiSchemaBuilder
{
    private static Dictionary<string, SchemaProperty> GetBaseCharacterLeadershipProperties()
    {
        return new Dictionary<string, SchemaProperty>
        {
            { "Name", new SchemaProperty { Type = "STRING", Description = "Character full name suitable for their race" } },
            { "Race", new SchemaProperty { Type = "STRING", Description = "Character race (Human, Elf, Dwarf, etc.)" } },
            { "Class", new SchemaProperty { Type = "STRING", Description = "D&D class or profession (Wizard, Rogue, Alchemist, Blacksmith)" } },
            { "Description", new SchemaProperty { Type = "STRING", Description = "Brief description of character appearance for players" } },
            { "ChallengeRating", new SchemaProperty { Type = "INTEGER", Description = "Character Challenge Rating value" } },
            { "ClassOrProfession", new SchemaProperty { Type = "STRING", Description = "D&D class or profession (Wizard, Rogue, Alchemist, Blacksmith)" } },
            { "Alignment", new SchemaProperty { Type = "STRING", Description = "Character alignment" } },
            { "HookOrSecret", new SchemaProperty { Type = "STRING", Description = "Character secret or quest hook" } }
        };
    }
    
    private static Dictionary<string, SchemaProperty> GetBaseCharacterProperties()
    {
        return new Dictionary<string, SchemaProperty>
        {
            { "Name", new SchemaProperty { Type = "STRING", Description = "Character full name suitable for their race" } },
            { "Race", new SchemaProperty { Type = "STRING", Description = "Character race (Human, Elf, Dwarf, etc.)" } },
            { "Class", new SchemaProperty { Type = "STRING", Description = "D&D class or profession (Wizard, Rogue, Alchemist, Blacksmith)" } },
            { "Description", new SchemaProperty { Type = "STRING", Description = "Brief description of character appearance for players" } },
            { "ChallengeRating", new SchemaProperty { Type = "INTEGER", Description = "Character Challenge Rating value" } },
            { "Alignment", new SchemaProperty { Type = "STRING", Description = "Character alignment" } },
            { "HookOrSecret", new SchemaProperty { Type = "STRING", Description = "Character secret or quest hook" } },
            { "Strength", new SchemaProperty { Type = "INTEGER", Description = "Strength attribute score from 8 to 15" } },
            { "Dexterity", new SchemaProperty { Type = "INTEGER", Description = "Dexterity attribute score from 8 to 15" } },
            { "Constitution", new SchemaProperty { Type = "INTEGER", Description = "Constitution attribute score from 8 to 15" } },
            { "Intelligence", new SchemaProperty { Type = "INTEGER", Description = "Intelligence attribute score from 8 to 15" } },
            { "Wisdom", new SchemaProperty { Type = "INTEGER", Description = "Wisdom attribute score from 8 to 15" } },
            { "Charisma", new SchemaProperty { Type = "INTEGER", Description = "Charisma attribute score from 8 to 15" } },
            { "Biography", new SchemaProperty 
                { 
                    Type = "OBJECT", 
                    Description = "Biographical data for Foundry VTT",
                    Properties = new Dictionary<string, SchemaProperty>
                    {
                        { "Gender", new SchemaProperty { Type = "STRING", Description = "Character gender" } },
                        { "Age", new SchemaProperty { Type = "STRING", Description = "Age" } },
                        { "Height", new SchemaProperty { Type = "STRING", Description = "Height" } },
                        { "Weight", new SchemaProperty { Type = "STRING", Description = "Weight" } },
                        { "Eyes", new SchemaProperty { Type = "STRING", Description = "Eye color" } },
                        { "Skin", new SchemaProperty { Type = "STRING", Description = "Skin tone" } },
                        { "Hair", new SchemaProperty { Type = "STRING", Description = "Hair style and color" } },
                        { "Faith", new SchemaProperty { Type = "STRING", Description = "Deity or faith" } },
                        { "Appearance", new SchemaProperty { Type = "STRING", Description = "Detailed description of clothing" } },
                        { "Background", new SchemaProperty { Type = "STRING", Description = "Life background story" } }
                    }
                } 
            },
            { "Journal", new SchemaProperty 
                { 
                    Type = "OBJECT", 
                    Description = "Dungeon Master notes",
                    Properties = new Dictionary<string, SchemaProperty>
                    {
                        { "PersonalOfInterest", new SchemaProperty { Type = "STRING", Description = "Persons of interest" } },
                        { "LocationOfInterest", new SchemaProperty { Type = "STRING", Description = "Locations of interest" } },
                        { "Quests", new SchemaProperty { Type = "STRING", Description = "Quests and rumors" } },
                        { "Miscellaneous", new SchemaProperty { Type = "STRING", Description = "Miscellaneous trivia and facts" } },
                        { "JournalEntries", new SchemaProperty { Type = "STRING", Description = "Journal entries" } }
                    }
                } 
            },
            { "InventoryTags", new SchemaProperty 
                { 
                    Type = "ARRAY",
                    Description = "List of required inventory tags for item generation.",
                    Items = new SchemaProperty
                    {
                        Type = "OBJECT",
                        Required = new List<string> { "Type", "Rarity" },
                        Properties = new Dictionary<string, SchemaProperty>
                        {
                            { "Type", new SchemaProperty { Type = "STRING", Description = "Strictly one of: weapon, consumable, equipment, loot, mgc" } },
                            { "Rarity", new SchemaProperty { Type = "STRING", Description = "Strictly one of: common, uncommon, rare, veryRare, legendary, artifact" } }
                        }
                    }
                }
            }
        };
    }

    public static ResponseSchema BuildSchemaForNpc()
    {
        return new ResponseSchema
        {
            Type = "OBJECT",
            Required = new List<string> { "Name", "Race", "Description", "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" },
            Properties = GetBaseCharacterProperties()
        };
    }

    public static ResponseSchema BuildSchemaForMerchant()
    {
        var properties = GetBaseCharacterProperties();

        properties.Add("ShopName", new SchemaProperty { Type = "STRING", Description = "Merchant shop name" });
        properties.Add("ShopDescription", new SchemaProperty { Type = "STRING", Description = "Shop description" });
        properties.Add("MerchantDescription", new SchemaProperty { Type = "STRING", Description = "Merchant appearance and personality description" });
        properties.Add("PriceModifier", new SchemaProperty { Type = "NUMBER", Description = "Price modifier multiplier (e.g. 1.0, 1.25, 0.85)" });

        return new ResponseSchema
        {
            Type = "OBJECT",
            Required = new List<string> { "Name", "Race", "Class", "Description", "ShopName", "ShopDescription", "MerchantDescription", "PriceModifier", "InventoryTags" },
            Properties = properties
        };
    }

    public static ResponseSchema BuildSchemaForFaction()
    {
        var properties = new Dictionary<string, SchemaProperty>
        {
            { "Name", new SchemaProperty { Type = "STRING", Description = "Faction name" } },
            { "Description", new SchemaProperty { Type = "STRING", Description = "Faction description" } },
            { "Goal", new SchemaProperty { Type = "STRING", Description = "Faction primary goal" } },
            { "Motivation", new SchemaProperty { Type = "STRING", Description = "Faction motivation" } },
            { "RelationshipToPlayer", new SchemaProperty { Type = "STRING", Description = "Faction relationship to player party" } },
            { "Reputation", new SchemaProperty { Type = "STRING", Description = "Faction reputation level or description" } },
            { "Headquarters", new SchemaProperty
                {
                    Type = "OBJECT",
                    Description = "Faction headquarters",
                    Properties = GetSchemaForHeadquarters().Properties
                }
            },
            { "Leader", new SchemaProperty
                {
                    Type = "OBJECT",
                    Description = "Faction leader",
                    Properties = GetBaseCharacterLeadershipProperties()
                }
            }
        };

        return new ResponseSchema
        {
            Type = "OBJECT",
            Required = new List<string>
            {
                "Name", "Description", "Goal", "Motivation", "RelationshipToPlayer", "Reputation", "Headquarters", "Leader"
            },
            Properties = properties
        };
    }

    public static ResponseSchema GetSchemaForHeadquarters()
    {
        var properties = new Dictionary<string, SchemaProperty>
        {
            { "Name", new SchemaProperty { Type = "STRING", Description = "Headquarters name" } },
            { "Description", new SchemaProperty { Type = "STRING", Description = "Headquarters description" } },
            { "Type", new SchemaProperty { Type = "STRING", Description = "Location type" } },
            { "Atmosphere", new SchemaProperty { Type = "STRING", Description = "Location atmosphere and sensory details" } }
        };

        return new ResponseSchema
        {
            Type = "OBJECT",
            Required = new List<string>
            {
                "Name", "Description", "Type", "Atmosphere"
            },
            Properties = properties
        };
    }
}