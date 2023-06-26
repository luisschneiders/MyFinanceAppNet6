using Microsoft.AspNetCore.Components;


namespace MainApp.Pages.SettingsPage;

public partial class SettingsAppearance : ComponentBase
{
    [Parameter]
    public EventCallback OnClickSuccess { get; set; }

    [Parameter]
    public string Radius { get; set; } = string.Empty;

    public SettingsAppearance()
    {
    }

    private async Task SetSettingsAppearance()
    {
        await OnClickSuccess.InvokeAsync();
        await Task.CompletedTask;
    }
}
