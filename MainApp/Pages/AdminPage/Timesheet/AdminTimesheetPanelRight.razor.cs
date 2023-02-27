using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetPanelRight : ComponentBase, IDisposable
{
    [Inject]
    private ITimesheetService<TimesheetModel> _timesheetService { get; set; } = default!;

    [Inject]
    private TimesheetStateService _timesheetStateService { get; set; } = default!;

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private TimesheetModel _timesheetModel { get; set; } = new();
    private TimesheetModelStateContainerDTO _timesheetModelStateContainerDTO { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    public AdminTimesheetPanelRight()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        _timesheetModel.TimeIn = DateTime.Now;
        _timesheetModel.TimeOut = DateTime.Now;

        _timesheetStateService.OnStateChange += StateHasChanged;

        _timesheetModelStateContainerDTO = _timesheetStateService.Value;

        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _spinnerService.ShowSpinner();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            StateHasChanged();
        }

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _timesheetStateService.OnStateChange -= StateHasChanged;
    }
}
