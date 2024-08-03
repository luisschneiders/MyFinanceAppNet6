namespace MainApp.Services;

public interface ILocalStorageService
{
    public Task<T> GetAsync<T>(string key);
    public Task RemoveAsync(string key);
    public Task SetAsync<T>(string key, T value);
    public Task ClearAsync();
}
