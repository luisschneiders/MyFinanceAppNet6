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

    private List<TimesheetModel> _timesheets { get; set; } = new();
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

    private async Task UpdateStatusAsync(TimesheetModel timesheetModel)
    {
        try
        {
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
        // TODO: add service to refresh the list
        TimesheetModel updatedModel = _setupOffCanvas.DataModel;
        TimesheetModel model = _timesheets.FirstOrDefault(b => b.Id == updatedModel.Id)!;

        var index = _timesheets.IndexOf(model);

        if (index != -1)
        {
            if (updatedModel.IsArchived)
            {
                var archivedModel = _timesheets[index] = updatedModel;
                _timesheets.Remove(archivedModel);
            }
            else
            {
                _timesheets[index] = updatedModel;
            }
        }
        else
        {
            _timesheets.Add(updatedModel);
        }

        await InvokeAsync(StateHasChanged);
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(TimesheetModel timesheetModel)
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
        _toastService.ShowToast("Date range has changed!", Theme.Info);

        //await RefreshVirtualizeContainer();
        await Task.CompletedTask;
    }
}
