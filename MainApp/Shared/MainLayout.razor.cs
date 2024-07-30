using Microsoft.AspNetCore.Components;

namespace MainApp.Shared;

public partial class MainLayout
{
    [CascadingParameter]
    protected AppSettings AppSettings { get; set; } = new();

    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    public MainLayout()
	{
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            AppSettings = await _appSettingsService.GetInterface();
            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }
}
