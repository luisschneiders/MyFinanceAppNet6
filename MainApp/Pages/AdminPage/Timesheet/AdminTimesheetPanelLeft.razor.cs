using Microsoft.AspNetCore.Components;
using MainApp.Pages.AdminPage.Timesheet;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetPanelLeft : ComponentBase
{
    [Inject]
    private ITimesheetService<TimesheetModel> _timesheetService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private ToastService _toastService { get; set; } = new();

    /*
     * Add OffCanvas component reference
     */
    private AdminTimesheetOffCanvas _setupOffCanvas { get; set; } = new();
    private TimesheetModel _timesheetModel { get; set; } = new();
    private DateTimeRangeModel _dateTimeRangeModel { get; set; } = new();

    private List<TimesheetModelListDTO> _timesheets { get; set; } = new();
    private bool _isLoading { get; set; } = true;

    public AdminTimesheetPanelLeft()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRangeModel = _dateTimeService.GetCurrentMonth();
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _spinnerService.ShowSpinner();
                await FetchDataAsync();
            }
            catch (Exception ex)
            {
                await Task.Delay((int)Delay.DataError);
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _timesheets = await _timesheetService.GetRecordsByDateRange(_dateTimeRangeModel);
            _isLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _isLoading = false;
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(TimesheetModelListDTO timesheetModelListDTO)
    {
        try
        {
            TimesheetModel timesheetModel = new()
            {
                Id = timesheetModelListDTO.Id,
                IsActive = timesheetModelListDTO.IsActive
            };
            await _timesheetService.UpdateRecordStatus(timesheetModel);
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task RefreshList()
    {
        await FetchDataAsync();
        StateHasChanged();
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(TimesheetModelListDTO timesheetModel)
    {
        try
        {
            await _setupOffCanvas.EditRecordOffCanvasAsync(timesheetModel.Id.ToString());
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(TimesheetModel timesheetModel)
    {
        try
        {
            await _setupOffCanvas.ViewRecordOffCanvasAsync(timesheetModel.Id.ToString());
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task RefreshListFromDropdownDateRange()
    {
        await FetchDataAsync();

        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await Task.CompletedTask;
    }
}
