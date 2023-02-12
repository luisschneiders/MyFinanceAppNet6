using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetPanelRight : ComponentBase
{
    private TimesheetModel _timesheetModel { get; set; } = new();

    public AdminTimesheetPanelRight()
    {
    }

    protected override async Task OnInitializedAsync()
    {

        _timesheetModel.TimeIn = DateTime.Now;
        _timesheetModel.TimeOut = DateTime.Now;

        await Task.CompletedTask;
    }
}
