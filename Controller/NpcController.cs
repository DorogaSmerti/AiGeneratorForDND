using Microsoft.AspNetCore.Mvc;
using StoryTracker.Models;
using StoryTracker.Service.Interface;

namespace StoryTracker.Controller;

[ApiController]
[Route("api/[controller]")]

public class NpcController : ControllerBase
{
    private readonly INpcService _npcService;
    private readonly INpcExportService _npcExportService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public NpcController(INpcService npcService, INpcExportService npcExportService, IWebHostEnvironment webHostEnvironment)
    {
        _npcService = npcService;
        _npcExportService = npcExportService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> NpcGenerate([FromBody]NpcRequest npcRequest)
    {
        var result = await _npcService.GenerateNpcAsync(npcRequest);

        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        string fvvtJson = await _npcExportService.ExportToFvttJsonAsync(result.Value!, "База");

        if(_webHostEnvironment.IsDevelopment())
        {
            await _npcExportService.ExportInJsonFile(fvvtJson, result.Value!);
            return Ok(result.Value);
        }

        byte[] fileByte = System.Text.Encoding.UTF8.GetBytes(fvvtJson);
        string fileName = $"{result.Value.Name ?? "Npc"}.json";

        return File(fileByte, "application/json", fileName);
    }

    [HttpPost("generate-merchant")]
    public async Task<IActionResult> MerchantGenerate([FromBody]MerchantRequest npcRequest)
    {
        var result = await _npcService.GenerateMerchantAsync(npcRequest);

        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        string fvvtJson = await _npcExportService.ExportToFvttJsonAsync(result.Value!, "Магазин");

        if(_webHostEnvironment.IsDevelopment())
        {
            await _npcExportService.ExportInJsonFile(fvvtJson, result.Value!);
            return Ok(result.Value);
        }

        byte[] fileByte = System.Text.Encoding.UTF8.GetBytes(fvvtJson);
        string fileName = $"{result.Value.Name ?? "Npc"}.json";

        return File(fileByte, "application/json", fileName);
    }
}