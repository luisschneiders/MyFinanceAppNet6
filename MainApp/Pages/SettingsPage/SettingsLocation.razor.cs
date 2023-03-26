using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsLocation : ComponentBase
{
    [Parameter]
    public EventCallback OnClickSuccess { get; set; }

    public SettingsLocation()
    {
    }

    private async Task SetSettingsLocation()
    {
        await OnClickSuccess.InvokeAsync();
        await Task.CompletedTask;
    }
}
