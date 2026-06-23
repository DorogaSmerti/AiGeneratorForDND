using StoryTracker.Models;

namespace StoryTracker.Service;

public static class AiSchemaBuilder
{
    public static ResponseSchema BuildSchema()
    {
        return new ResponseSchema
        {
            Type = "OBJECT",
            
            Required = new List<string> { "Name", "Race", "Description", "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" },
            
            Properties = new Dictionary<string, SchemaProperty>
            {
                { "Name", new SchemaProperty { Type = "STRING", Description = "Имя и фамилия NPC, подходящие его расе" } },
                { "Race", new SchemaProperty { Type = "STRING", Description = "Раса персонажа (Эльф, Дварф, Человек и т.д.)" } },
                { "Class", new SchemaProperty { Type = "STRING", Description = "Класс персонажа из официальной книги D&D (Wizard, Warrior и т.д.)"}},
                { "Description", new SchemaProperty { Type = "STRING", Description = "Краткое описание внешности НПС, которое можно зачитать для игроков" } },
                { "ImageUrl", new SchemaProperty { Type = "STRING", Description = "Оставь это поле абсолютно пустым словом ''" } },
                { "HookOrSecret", new SchemaProperty { Type = "STRING", Description = "Тайна персонажа или зацепка для квеста, которую знает только Мастер" } },

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
                            { "Age", new SchemaProperty { Type = "STRING", Description = "Возраст (например: 25 лет, 150 лет для эльфа)" } },
                            { "Height", new SchemaProperty { Type = "STRING", Description = "Рост персонажа (например: 180 см)" } },
                            { "Weight", new SchemaProperty { Type = "STRING", Description = "Вес персонажа (например: 75 кг)" } },
                            { "Eyes", new SchemaProperty { Type = "STRING", Description = "Цвет глаз" } },
                            { "Skin", new SchemaProperty { Type = "STRING", Description = "Цвет кожи или оттенок" } },
                            { "Hair", new SchemaProperty { Type = "STRING", Description = "Цвет и стиль прически/бороды" } },
                            { "Faith", new SchemaProperty { Type = "STRING", Description = "Божество, в которое верит NPC, или идеалы" } },
                            { "Appearance", new SchemaProperty { Type = "STRING", Description = "Детальное описание одежды и приметных черт" } },
                            { "Background", new SchemaProperty { Type = "STRING", Description = "История жизни персонажа, кем он был до этого" } }
                        }
                    } 
                },

                { "Journal", new SchemaProperty 
                    { 
                        Type = "OBJECT", 
                        Description = "Заметки Мастера об отношениях этого NPC с миром",
                        Properties = new Dictionary<string, SchemaProperty>
                        {
                            { "PersonalOfInterest", new SchemaProperty { Type = "STRING", Description = "Важные для этого NPC личности (друзья, враги)" } },
                            { "LocationOfInterest", new SchemaProperty { Type = "STRING", Description = "Места, с которыми связан NPC (его лавка, родной город)" } },
                            { "Quests", new SchemaProperty { Type = "STRING", Description = "Какие поручения или слухи может дать этот NPC игрокам" } },
                            { "Miscellaneous", new SchemaProperty { Type = "STRING", Description = "Дополнительные интересные факты" } },
                            { "JournalEntries", new SchemaProperty { Type = "STRING", Description = "Личные дневниковые записи или слухи об этом персонаже" } }
                        }
                    } 
                },

                { "InventoryTags", new SchemaProperty 
                    { 
                        Type = "ARRAY", 
                        Description = "A list of required item tags to generate the NPC's equipment.",
                        Items = new SchemaProperty
                        { 
                            Type = "OBJECT",
                            Properties = new Dictionary<string, SchemaProperty>
                            {
                                { "Type", new SchemaProperty { Type = "STRING", Description = "Строго (weapon, consumable, equipment, magic)" } },
                                { "Rarity", new SchemaProperty { Type = "STRING", Description = "Строго (common, uncommon, rare, very rare, mythic, legendary, artifact)" } }
                            }
                        }
                    }
                }
            }
        };
    }
}
        