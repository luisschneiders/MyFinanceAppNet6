using System.Text.Json;
using Microsoft.JSInterop;

namespace MainApp.Services;

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
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

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", new object[] { key, jsValue });
    }

    public async Task<T> GetAsync<T>(string key)
    {
        string val = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

        if (val is null)
        {
            return default!;
        }

        T result = JsonSerializer.Deserialize<T>(val)!;

        return result;
    }

    public async Task RemoveAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }
}
