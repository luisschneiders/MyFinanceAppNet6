
using MainApp.Components.Toast;
using Microsoft.JSInterop;

namespace MainApp.Services;

public class AnimationService : IAnimationService
{
    private readonly IJSRuntime _jsRuntime;
    private ToastService _toastService { get; set; } = default!;

    public AnimationService(IJSRuntime jSRuntime, ToastService toastService)
    {
        _jsRuntime = jSRuntime;
        _toastService = toastService;
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
            _toastService.ShowToast(Label.AppMessageConfettiNotDefined, Theme.Danger);
        }
    }
}
