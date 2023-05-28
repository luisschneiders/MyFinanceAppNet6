namespace MainApp.Services;

public interface ILocalStorageService
{
    Task<T> GetAsync<T>(string key);
    Task RemoveAsync(string key);
    Task SetAsync<T>(string key, T value);
}
