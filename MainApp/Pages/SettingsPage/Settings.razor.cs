using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class Settings : ComponentBase
{
    /*
     * Add component reference
     */
    SettingsAppearanceOffCanvas _settingsAppearanceOffCanvas { get; set; } = new();

    public Settings()
    {
    }

    private async Task OpenAppearanceOffCanvas()
    {
        await Task.FromResult(_settingsAppearanceOffCanvas.OpenAsync());
        await Task.CompletedTask;
    }
}
