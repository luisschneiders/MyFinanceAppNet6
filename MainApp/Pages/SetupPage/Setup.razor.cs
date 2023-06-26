using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage;

public partial class Setup : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    private string _radius { get; set; } = Radius.Default;

    public Setup()
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
}
