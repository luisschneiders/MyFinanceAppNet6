using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Map;

public class MapService : IMapService
{
    [Inject]
    private IJSRuntime _js { get; set; } = default!;

    [Inject]
    private IGoogleService _googleService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    public MapService(IJSRuntime JS, IGoogleService googleService)
    {
        _js = JS;
        _googleService = googleService;
    }
    public async Task InjectGoogleInteractiveMap()
    {
        try
        {
            Response<Uri> response = await _googleService.GetMapInteractiveUrl();

            if (response.Success is false)
            {
                _toastService.ShowToast($"{response.ErrorMessage}", Theme.Danger);
            }
            else{
                await _js.InvokeVoidAsync("addScriptInteractiveMapGoogle", response.Data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
