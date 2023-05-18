using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage;

public partial class Admin : ComponentBase
{
    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();

    public Admin()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentYear();
        await Task.CompletedTask;
    }
}
