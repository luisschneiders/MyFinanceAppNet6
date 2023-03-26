using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsLocationOffCanvas : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    private OffCanvas _offCanvas { get; set; } = new();
    private string _offCanvasTarget { get; set; } = string.Empty;

    private LocationModel _locationModel { get; set; } = new();

    private bool _isProcessing { get; set; } = false;
    private bool _isVerified { get; set; } = false;

    public SettingsLocationOffCanvas()
    {
    }

    public async Task OpenAsync()
    {
        _offCanvasTarget = Guid.NewGuid().ToString();

        await Task.FromResult(_offCanvas.Open(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        await Task.FromResult(_offCanvas.Close(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task VerifyAddress()
    {
        try
        {
            _toastService.ShowToast("Verifying", Theme.Info);

        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task SaveAddress()
    {
        try
        {
            _toastService.ShowToast("Saving", Theme.Info);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }
}
