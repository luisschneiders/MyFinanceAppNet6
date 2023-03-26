using MainApp.Components.OffCanvas;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsAppearanceOffCanvas : ComponentBase
{
    private OffCanvas _offCanvas { get; set; } = new();
    private string _offCanvasTarget { get; set; } = string.Empty;

    public SettingsAppearanceOffCanvas()
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

    private async Task ChangeColorModeAsync(ColorMode color)
    {
        await JS.InvokeVoidAsync("updateColorMode", color.ToString().ToLower());
        await Task.CompletedTask;
    }
}
