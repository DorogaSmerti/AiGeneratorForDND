using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using StoryTracker.Models;
using StoryTracker.Service;
using StoryTracker.Service.Interface;
using Xunit;

namespace StoryTracker.Tests;

public class NpcServiceTest
{
    protected readonly IAiService _aiService = Substitute.For<IAiService>();
    protected readonly IItemService _itemService = Substitute.For<IItemService>();
    protected readonly IConfiguration _configuration = Substitute.For<IConfiguration>();
    protected readonly IGeneratePromts _generatePromts = Substitute.For<IGeneratePromts>();
    protected readonly ILogger<NpcService> _logger = Substitute.For<ILogger<NpcService>>();
    protected readonly INpcService _sut;

    public NpcServiceTest()
    {
        _configuration["AvatarSettings:AvatarPath"].Returns("https://raw.githubusercontent.com/DorogaSmerti/avatars_DND/main/avatars/");
        _sut = new NpcService(_aiService, _itemService, _generatePromts, _configuration, _logger);

    }

    [Fact]
    public async Task GenerateNpcAsync_WhenRequestIsNull_ReturnsInvalidRequestFailure()
    {
        var result = await _sut.GenerateNpcAsync(null!);

        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Gpt.InvalidRequest, result.Error);
    }

    [Fact]
    public async Task GenerateNpcAsync_WhenSuccess_ReturnsNpc()
    {
        var request = new NpcRequest { Name = "Arthas", Race = "Human", ClassOrProfession = "Paladin" };
        var npc = new BaseCharacter { Name = "Arthas", Race = "Human", Class = "Paladin" };

        _aiService.SendRequestToGeminiAsync<BaseCharacter>(Arg.Any<string>(), Arg.Any<ResponseSchema>())
        .Returns(Result<BaseCharacter>.Success(npc));

        var result = await _sut.GenerateNpcAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(request.Name, result.Value?.Name);
        Assert.Equal(request.ClassOrProfession, result.Value?.Class);
        Assert.Equal(request.Race, result.Value?.Race);
        Assert.Equal("https://raw.githubusercontent.com/DorogaSmerti/avatars_DND/main/avatars/paladin.png", result.Value?.ImagePath);
    }
}