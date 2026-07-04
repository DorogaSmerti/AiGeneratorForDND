using Scalar.AspNetCore;
using StoryTracker.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IGeneratePromts, GeneratePromts>();
builder.Services.AddScoped<INpcService, NpcService>();
builder.Services.AddScoped<INpcExportService, NpcExportService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddSingleton<IITemDataStorage, ItemDataStorage>();

builder.Services.AddHttpClient<NpcService>();

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(Options =>
    {
        Options.WithOpenApiRoutePattern("/openapi/v1.json");
    });
}

app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.Run();

