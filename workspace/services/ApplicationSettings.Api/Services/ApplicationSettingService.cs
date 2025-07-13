using ApplicationSettings.Api.Configuration;
using ApplicationSettings.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApplicationSettings.Api.Services;

public class ApplicationSettingService : IApplicationSettingService
{
    private readonly IMongoCollection<ApplicationSetting> _settings;

    public ApplicationSettingService(IOptions<MongoDbOptions> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var database = mongoClient.GetDatabase(options.Value.DatabaseName);
        _settings = database.GetCollection<ApplicationSetting>(options.Value.CollectionName);
    }

    public async Task<IEnumerable<ApplicationSetting>> GetAllAsync()
    {
        return await _settings.Find(_ => true).ToListAsync();
    }

    public async Task<ApplicationSetting?> GetByKeyAsync(string key)
    {
        return await _settings.Find(x => x.Key == key).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ApplicationSetting>> GetByCategoryAsync(string category)
    {
        return await _settings.Find(x => x.Category == category).ToListAsync();
    }

    public async Task<ApplicationSetting> CreateAsync(ApplicationSetting setting)
    {
        setting.CreatedAt = DateTime.UtcNow;
        setting.UpdatedAt = DateTime.UtcNow;
        await _settings.InsertOneAsync(setting);
        return setting;
    }

    public async Task<ApplicationSetting?> UpdateAsync(string key, ApplicationSetting setting)
    {
        setting.UpdatedAt = DateTime.UtcNow;
        var result = await _settings.ReplaceOneAsync(x => x.Key == key, setting);
        return result.MatchedCount > 0 ? setting : null;
    }

    public async Task<bool> DeleteAsync(string key)
    {
        var result = await _settings.DeleteOneAsync(x => x.Key == key);
        return result.DeletedCount > 0;
    }
}