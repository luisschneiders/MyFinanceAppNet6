using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Constants;

namespace MainApp.Shared;

public partial class LoginDisplay : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    private string _radius { get; set; } = Radius.Default;

    public LoginDisplay()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _radius = await _appSettingsService.GetButtonShape();
            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }
}
