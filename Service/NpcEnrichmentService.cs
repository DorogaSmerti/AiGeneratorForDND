using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Service;

public class NpcEnrichmentService : INpcEnrichmentService
{
    private readonly IItemService _itemService;
    private readonly IConfiguration _configuration;
    private readonly string _baseAvatarUrlPath;

    public NpcEnrichmentService(IConfiguration configuration, IItemService itemService)
    {
        _itemService = itemService;
        _configuration = configuration;
        _baseAvatarUrlPath = _configuration["AvatarSettings:AvatarPath"] ?? throw new ArgumentNullException("UrlPath configuration is missing.");
    }

    public async Task MappingInventoryAsync(BaseCharacter baseCharacter)
    {
        if(baseCharacter.InventoryTags == null) return;

        foreach(var item in baseCharacter.InventoryTags)
        {
            InventoryGenerationRequest generationRequest = new InventoryGenerationRequest
            {
                ClassName = baseCharacter.Class,
                Rarity = item.Rarity,
                Type = item.Type
            };

            var generatedItem = await _itemService.GetItemFromLocalDump(generationRequest);
            if (generatedItem.IsSuccess)
            {
                baseCharacter.InventoryDto.Add(generatedItem.Value!.DeepClone());
            }
        }
    }

    public string? GetAvatarFromDump(string? npcClass)
    {
        if (string.IsNullOrWhiteSpace(npcClass))
        {
            return Path.Combine(_baseAvatarUrlPath, "default_npc.png"); 
        };

        string fileName = $"{npcClass.ToLower()}.png";

        string fullPath = Path.Combine(_baseAvatarUrlPath, fileName);

        return fullPath;
    }

}