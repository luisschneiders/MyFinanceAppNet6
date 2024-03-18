
using Microsoft.JSInterop;

namespace MainApp.Services;

public class BrowserService : IBrowserService
{
    private readonly IJSRuntime _jsRuntime;

    public BrowserService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    public async Task CloseTab()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("closeBrowserTab");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
