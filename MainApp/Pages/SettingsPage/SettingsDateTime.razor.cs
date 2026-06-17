using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsDateTime : ComponentBase
{
    [Parameter]
    public EventCallback OnClickSuccess { get; set; }

    [Parameter]
    public string Radius { get; set; } = string.Empty;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    public SettingsDateTime()
    {
    }

    private async Task SetSettingsDateTime()
    {
        await OnClickSuccess.InvokeAsync();
        await Task.CompletedTask;
    }
}
