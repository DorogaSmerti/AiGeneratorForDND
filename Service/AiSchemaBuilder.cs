using StoryTracker.Models;

namespace StoryTracker.Service;

public static class AiSchemaBuilder
{
    private static Dictionary<string, SchemaProperty> GetBaseCharacterLeadershipProperties()
    {
        return new Dictionary<string, SchemaProperty>
        {
            { "Name", new SchemaProperty { Type = "STRING", Description = "Имя и фамилия персонажа, подходящие его расе" } },
            { "Race", new SchemaProperty { Type = "STRING", Description = "Раса персонажа (Человек, Эльф, Дварф и т.д.)" } },
            { "Class", new SchemaProperty { Type = "STRING", Description = "Класс/профессия персонажа из D&D (Wizard, Rogue, Alchemist, Blacksmith)" } },
            { "Description", new SchemaProperty { Type = "STRING", Description = "Краткое описание внешности персонажа для игроков" } },
            { "ChallengeRating", new SchemaProperty { Type = "INTEGER", Description = "Значение Challenge Rating персонажа" } },
            { "ClassOrProfession", new SchemaProperty { Type = "STRING", Description = "Класс/профессия персонажа из D&D (Wizard, Rogue, Alchemist, Blacksmith)" } }
            
        };
    }
    
    private static Dictionary<string, SchemaProperty> GetBaseCharacterProperties()
    {
        return new Dictionary<string, SchemaProperty>
        {
            { "Name", new SchemaProperty { Type = "STRING", Description = "Имя и фамилия персонажа, подходящие его расе" } },
            { "Race", new SchemaProperty { Type = "STRING", Description = "Раса персонажа (Человек, Эльф, Дварф и т.д.)" } },
            { "Class", new SchemaProperty { Type = "STRING", Description = "Класс/профессия персонажа из D&D (Wizard, Rogue, Alchemist, Blacksmith)" } },
            { "Description", new SchemaProperty { Type = "STRING", Description = "Краткое описание внешности персонажа для игроков" } },
            { "ChallengeRating", new SchemaProperty { Type = "INTEGER", Description = "Значение Challenge Rating персонажа" } },
            { "Alignment", new SchemaProperty { Type = "STRING", Description = "Мировоззрение персонажа" } },
            { "HookOrSecret", new SchemaProperty { Type = "STRING", Description = "Тайна или квестовая зацепка персонажа" } },
            { "Strength", new SchemaProperty { Type = "INTEGER", Description = "Сила от 8 до 15" } },
            { "Dexterity", new SchemaProperty { Type = "INTEGER", Description = "Ловкость от 8 до 15" } },
            { "Constitution", new SchemaProperty { Type = "INTEGER", Description = "Телосложение от 8 до 15" } },
            { "Intelligence", new SchemaProperty { Type = "INTEGER", Description = "Интеллект от 8 до 15" } },
            { "Wisdom", new SchemaProperty { Type = "INTEGER", Description = "Мудрость от 8 до 15" } },
            { "Charisma", new SchemaProperty { Type = "INTEGER", Description = "Харизма от 8 до 15" } },
            { "Biography", new SchemaProperty 
                { 
                    Type = "OBJECT", 
                    Description = "Биографические данные для Foundry VTT",
                    Properties = new Dictionary<string, SchemaProperty>
                    {
                        { "Gender", new SchemaProperty { Type = "STRING", Description = "Пол персонажа" } },
                        { "Age", new SchemaProperty { Type = "STRING", Description = "Возраст" } },
                        { "Height", new SchemaProperty { Type = "STRING", Description = "Рост" } },
                        { "Weight", new SchemaProperty { Type = "STRING", Description = "Вес" } },
                        { "Eyes", new SchemaProperty { Type = "STRING", Description = "Цвет глаз" } },
                        { "Skin", new SchemaProperty { Type = "STRING", Description = "Цвет кожи" } },
                        { "Hair", new SchemaProperty { Type = "STRING", Description = "Волосы/прическа" } },
                        { "Faith", new SchemaProperty { Type = "STRING", Description = "Вера/божество" } },
                        { "Appearance", new SchemaProperty { Type = "STRING", Description = "Детальное описание одежды" } },
                        { "Background", new SchemaProperty { Type = "STRING", Description = "История жизни" } }
                    }
                } 
            },
            { "Journal", new SchemaProperty 
                { 
                    Type = "OBJECT", 
                    Description = "Заметки Мастера",
                    Properties = new Dictionary<string, SchemaProperty>
                    {
                        { "PersonalOfInterest", new SchemaProperty { Type = "STRING", Description = "Связанные личности" } },
                        { "LocationOfInterest", new SchemaProperty { Type = "STRING", Description = "Связанные локации" } },
                        { "Quests", new SchemaProperty { Type = "STRING", Description = "Поручения/слухи" } },
                        { "Miscellaneous", new SchemaProperty { Type = "STRING", Description = "Интересные факты" } },
                        { "JournalEntries", new SchemaProperty { Type = "STRING", Description = "Записи дневника" } }
                    }
                } 
            },
            { "InventoryTags", new SchemaProperty 
                { 
                    Type = "ARRAY",
                    Description = "Список требуемых тегов для генерации предметов.",
                    Items = new SchemaProperty
                    {
                        Type = "OBJECT",
                        Required = new List<string> { "Type", "Rarity" },
                        Properties = new Dictionary<string, SchemaProperty>
                        {
                            { "Type", new SchemaProperty { Type = "STRING", Description = "Строго одно: weapon, consumable, equipment, loot, mgc" } },
                            { "Rarity", new SchemaProperty { Type = "STRING", Description = "Строго одно: common, uncommon, rare, veryRare, legendary, artifact" } }
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

        properties.Add("ShopName", new SchemaProperty { Type = "STRING", Description = "Название торговой лавки" });
        properties.Add("ShopDescription", new SchemaProperty { Type = "STRING", Description = "Описание лавки" });
        properties.Add("MerchantDescription", new SchemaProperty { Type = "STRING", Description = "Описание внешности и характера торговца" });
        properties.Add("PriceModifier", new SchemaProperty { Type = "NUMBER", Description = "Множитель цены в лавке (например, 1.0, 1.25, 0.85)" });

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
            { "Name", new SchemaProperty { Type = "STRING", Description = "Название фракции" } },
            { "Description", new SchemaProperty { Type = "STRING", Description = "Описание фракции" } },
            { "Goal", new SchemaProperty { Type = "STRING", Description = "Цель фракции" } },
            { "Motivation", new SchemaProperty { Type = "STRING", Description = "Мотивация фракции" } },
            { "RelationshipToPlayer", new SchemaProperty { Type = "STRING", Description = "Отношение фракции к игроку" } },
            { "Reputation", new SchemaProperty { Type = "INTEGER", Description = "Репутация фракции" } },
            { "Headquarters", new SchemaProperty { Type = "STRING", Description = "Штаб-квартира фракции" } },
            { "Leader", new SchemaProperty {Type = "STRING", Description = "Лидер фракции"}}
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
}