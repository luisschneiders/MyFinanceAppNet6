using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpensePanelRight : ComponentBase
{
    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();

    public AdminExpensePanelRight()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        await Task.CompletedTask;
    }
}
