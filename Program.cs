using Scalar.AspNetCore;
using StoryTracker.Service;
using StoryTracker.Service.Interface;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddLogging(options =>
    options.AddConsole());

builder.Services.AddOpenApi();

builder.Services.AddScoped<IGeneratePromts, GeneratePromts>();
builder.Services.AddScoped<INpcService, NpcService>();
builder.Services.AddScoped<INpcExportService, NpcExportService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IFactionService, FactionService>();
builder.Services.AddScoped<IVectorService, VectorService>();
builder.Services.AddSingleton<IItemDataStorage, ItemDataStorage>();

builder.Services.AddHttpClient<IAiService, AiService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(Options =>
{
    Options.WithOpenApiRoutePattern("/openapi/v1.json");
});

app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.Run();

