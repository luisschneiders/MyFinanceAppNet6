using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Constants;

namespace MainApp.Pages.SettingsPage;

public partial class Settings : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    /*
     * Add component reference
     */
    SettingsAppearanceOffCanvas _settingsAppearanceOffCanvas { get; set; } = new();
    SettingsLocationOffCanvas _settingsLocationOffCanvas { get; set; } = new();
    SettingsDateTimeOffCanvas _settingsDateTimeOffCanvas { get; set; } = new();
    SettingsInterfaceOffCanvas _settingsInterfaceOffCanvas { get; set; } = new();

    private string _radius { get; set; } = Radius.Default;

    public Settings()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _radius = await _appSettingsService.GetCardShape();
            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }

    private async Task OpenAppearanceOffCanvas()
    {
        await Task.FromResult(_settingsAppearanceOffCanvas.OpenAsync());
        await Task.CompletedTask;
    }

    private async Task OpenLocationOffCanvas()
    {
        await Task.FromResult(_settingsLocationOffCanvas.OpenAsync());
        await Task.CompletedTask;
    }

    private async Task OpenDateTimeOffCanvas()
    {
        await Task.FromResult(_settingsDateTimeOffCanvas.OpenAsync());
        await Task.CompletedTask;
    }

    private async Task OpenInterfaceOffCanvas()
    {
        await Task.FromResult(_settingsInterfaceOffCanvas.OpenAsync());
        await Task.CompletedTask;
    }
}
