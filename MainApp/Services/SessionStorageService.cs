using System.Text.Json;
using Microsoft.JSInterop;

namespace MainApp.Services;

public class SessionStorageService : ISessionStorageService
{
    private readonly IJSRuntime _jsRuntime;
    public SessionStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetAsync<T>(string key, T value)
    {
        string jsValue = string.Empty;

        if (value != null)
        {
            jsValue = JsonSerializer.Serialize(value);
        }

        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", new object[] { key, jsValue });
    }

    public async Task<T> GetAsync<T>(string key)
    {
        string val = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);

        if (val is null)
        {
            return default!;
        }

        T result = JsonSerializer.Deserialize<T>(val)!;

        return result;
    }

    public async Task RemoveAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.clear");
    }
}
