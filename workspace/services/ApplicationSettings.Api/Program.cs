using ApplicationSettings.Api.Configuration;
using ApplicationSettings.Api.Models;
using ApplicationSettings.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.Configure<MongoDbOptions>(
    builder.Configuration.GetSection(MongoDbOptions.SectionName));

// Add services to the container
builder.Services.AddScoped<IApplicationSettingService, ApplicationSettingService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:80")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseHttpsRedirection();

// Application Settings endpoints (routes will be prefixed by gateway)
app.MapGet("/", async (IApplicationSettingService service) =>
{
    var settings = await service.GetAllAsync();
    return Results.Ok(settings);
})
.WithName("GetAllSettings")
.WithTags("Settings")
.WithOpenApi();

app.MapGet("/{key}", async (string key, IApplicationSettingService service) =>
{
    var setting = await service.GetByKeyAsync(key);
    return setting is not null ? Results.Ok(setting) : Results.NotFound();
})
.WithName("GetSettingByKey")
.WithTags("Settings")
.WithOpenApi();

app.MapGet("/category/{category}", async (string category, IApplicationSettingService service) =>
{
    var settings = await service.GetByCategoryAsync(category);
    return Results.Ok(settings);
})
.WithName("GetSettingsByCategory")
.WithTags("Settings")
.WithOpenApi();

app.MapPost("/", async (ApplicationSetting setting, IApplicationSettingService service) =>
{
    // Check if setting with this key already exists
    var existingSetting = await service.GetByKeyAsync(setting.Key);
    if (existingSetting is not null)
    {
        return Results.Conflict("Setting with this key already exists");
    }
    
    var createdSetting = await service.CreateAsync(setting);
    return Results.Created($"/{createdSetting.Key}", createdSetting);
})
.WithName("CreateSetting")
.WithTags("Settings")
.WithOpenApi();

app.MapPut("/{key}", async (string key, ApplicationSetting setting, IApplicationSettingService service) =>
{
    setting.Key = key; // Ensure the key matches the route parameter
    var updatedSetting = await service.UpdateAsync(key, setting);
    return updatedSetting is not null ? Results.Ok(updatedSetting) : Results.NotFound();
})
.WithName("UpdateSetting")
.WithTags("Settings")
.WithOpenApi();

app.MapDelete("/{key}", async (string key, IApplicationSettingService service) =>
{
    var deleted = await service.DeleteAsync(key);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteSetting")
.WithTags("Settings")
.WithOpenApi();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
.WithName("HealthCheck")
.WithTags("Health")
.WithOpenApi();

app.Run();
