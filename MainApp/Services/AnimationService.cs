
using Microsoft.JSInterop;

namespace MainApp.Services;

public class AnimationService : IAnimationService
{
    private readonly IJSRuntime _jsRuntime;

    public AnimationService(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;
    }
    public async Task ConfettiTransaction()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("startAnimation");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
