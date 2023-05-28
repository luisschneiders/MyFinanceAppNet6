using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsDateTime : ComponentBase
{
    [Parameter]
    public EventCallback OnClickSuccess { get; set; }

    public SettingsDateTime()
    {
    }

    private async Task SetSettingsDateTime()
    {
        await OnClickSuccess.InvokeAsync();
        await Task.CompletedTask;
    }
}
