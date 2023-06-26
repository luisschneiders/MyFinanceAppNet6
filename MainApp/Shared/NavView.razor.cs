using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Constants;

namespace MainApp.Shared;

public partial class NavView : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    private string _radius { get; set; } = Radius.Default;

    public NavView()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _radius = await _appSettingsService.GetMenuShape();
            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }

}
