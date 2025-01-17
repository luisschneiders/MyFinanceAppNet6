using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsInterfaceOffCanvas : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    /*
     * Add Modal component reference
     */
    private SettingsInterfaceModal _settingsInterfaceModal { get; set; } = new();

    private OffCanvas _offCanvas { get; set; } = new();
    private string _offCanvasTarget { get; set; } = string.Empty;
    private bool _interfaceChanged { get; set; } = false;

    public SettingsInterfaceOffCanvas()
	{
	}

    public async Task OpenAsync()
    {
        _offCanvasTarget = Guid.NewGuid().ToString();

        await Task.FromResult(_offCanvas.Open(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task SetRadiusAsync(string radius)
    {
        _interfaceChanged = true;
        await _appSettingsService.SetShapes(radius);
        await Task.CompletedTask;
    }

    private async Task SetShadowAsync(string shadow)
    {
        _interfaceChanged = true;
        await _appSettingsService.SetShadow(shadow);
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {

        await Task.FromResult(_offCanvas.Close(_offCanvasTarget));

        if (_interfaceChanged)
        {
            await Task.Delay((int)Delay.DataLoading);
            await ReloadAsync();
        }

        await Task.CompletedTask;
    }

    private async Task ReloadAsync()
    {
        await _settingsInterfaceModal.OpenModalAsync();
    }
}
