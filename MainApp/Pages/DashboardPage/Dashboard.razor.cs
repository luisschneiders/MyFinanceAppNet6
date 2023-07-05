using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Constants;

namespace MainApp.Pages.DashboardPage;

public partial class Dashboard : ComponentBase
{
    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    private DateTimeRange _dateTimeRange { get; set; } = new();

    public Dashboard()
    {        
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentYear();
        await Task.CompletedTask;
    }
}
