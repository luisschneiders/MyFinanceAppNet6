using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class TimesheetCalculator : ComponentBase
{
    private TimesheetModel _timesheetModel { get; set; } = new();

    public TimesheetCalculator()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        _timesheetModel.TimeIn = DateTime.Now;
        _timesheetModel.TimeOut = DateTime.Now;

        await Task.CompletedTask;
    }

}
