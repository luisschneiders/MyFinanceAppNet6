namespace MainApp.Services;

public interface ISessionStorageService
{
    Task<T> GetAsync<T>(string key);
    Task RemoveAsync(string key);
    Task SetAsync<T>(string key, T value);
    Task ClearAsync();
}
