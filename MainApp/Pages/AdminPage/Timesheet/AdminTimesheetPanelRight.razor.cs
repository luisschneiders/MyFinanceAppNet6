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
        // testing start
        var timein = DateTime.Parse("2023-02-08 8:00:00");
        var timebreak = TimeSpan.Parse("00:30:00");
        var timeout = DateTime.Parse("2023-02-08 19:30:00");

        _timesheetModel.TimeIn = timein;
        _timesheetModel.TimeBreak = timebreak;
        _timesheetModel.TimeOut = timeout;
        _timesheetModel.HourRate = (decimal)88.88;
        // testing end

        await Task.CompletedTask;
    }
}
