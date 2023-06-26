using Microsoft.AspNetCore.Components;

namespace MainApp.Shared;

public partial class LoginOrRegister : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    private string _radius { get; set; } = Radius.Default;

    public LoginOrRegister()
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
