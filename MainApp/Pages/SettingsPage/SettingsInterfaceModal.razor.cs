using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsInterfaceModal : ComponentBase
{
    [Inject]
    NavigationManager? _navManager { get; set; }

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }

    public SettingsInterfaceModal()
	{
	}

    public async Task OpenModalAsync()
    {
        try
        {
            _modalTarget = Guid.NewGuid();

            await Task.FromResult(_modal.Open(_modalTarget));
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ReloadAppAsync()
    {
        await Task.FromResult(_modal.Close(_modalTarget));
        _navManager?.NavigateTo(_navManager.Uri, forceLoad: true);
        await Task.CompletedTask;
    }
}
