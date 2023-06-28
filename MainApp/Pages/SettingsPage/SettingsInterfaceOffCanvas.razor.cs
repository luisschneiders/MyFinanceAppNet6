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
    protected AppSettings _appSettings { get; set; } = new();

    private OffCanvas _offCanvas { get; set; } = new();
    private string _offCanvasTarget { get; set; } = string.Empty;

    private string _radius { get; set; } = Radius.Default;

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
        _radius = radius;
        await _appSettingsService.SetShapes(radius);
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        await Task.FromResult(_offCanvas.Close(_offCanvasTarget));
        await Task.CompletedTask;
    }
}
