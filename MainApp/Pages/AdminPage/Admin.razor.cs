using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage;

public partial class Admin : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();

    private string _radius { get; set; } = Radius.Default;

    public Admin()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentYear();
        await Task.CompletedTask;
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
