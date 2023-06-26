using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsInterface : ComponentBase
{
    [Parameter]
    public EventCallback OnClickSuccess { get; set; }

    public SettingsInterface()
	{
	}

    private async Task SetSettingsButtonsForms()
    {
        await OnClickSuccess.InvokeAsync();
        await Task.CompletedTask;
    }
}
