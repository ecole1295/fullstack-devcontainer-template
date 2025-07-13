using ApplicationSettings.Api.Models;

namespace ApplicationSettings.Api.Services;

public interface IApplicationSettingService
{
    Task<IEnumerable<ApplicationSetting>> GetAllAsync();
    Task<ApplicationSetting?> GetByKeyAsync(string key);
    Task<IEnumerable<ApplicationSetting>> GetByCategoryAsync(string category);
    Task<ApplicationSetting> CreateAsync(ApplicationSetting setting);
    Task<ApplicationSetting?> UpdateAsync(string key, ApplicationSetting setting);
    Task<bool> DeleteAsync(string key);
}