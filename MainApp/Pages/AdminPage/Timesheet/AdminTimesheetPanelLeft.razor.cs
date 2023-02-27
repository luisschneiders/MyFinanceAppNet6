using Microsoft.AspNetCore.Components;
using MainApp.Pages.AdminPage.Timesheet;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MyFinanceAppLibrary.Constants;
using MainApp.StateServices;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetPanelLeft : ComponentBase, IDisposable
{
    [Inject]
    private ITimesheetService<TimesheetModel> _timesheetService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ICompanyService<CompanyModel> _companyService { get; set; } = default!;

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private TimesheetStateService _timesheetStateService { get; set; } = default!;

    [Parameter]
    public EventCallback OnStateContainerSetValue { get; set; }

    /*
     * Add OffCanvas component reference
     */
    private AdminTimesheetOffCanvas _setupOffCanvas { get; set; } = new();

    private TimesheetModel _timesheetModel { get; set; } = new();
    private DateTimeRangeModel _dateTimeRangeModel { get; set; } = new();

    private List<TimesheetModelListDTO> _timesheets { get; set; } = new();
    private List<CompanyModel> _companies { get; set; } = new();
    private TimesheetModelStateContainerDTO _timesheetModelStateContainerDTO { get; set; } = new();

    private PayStatus[] _payStatuses { get; set; } = default!;

    private bool _isLoading { get; set; } = true;

    private decimal _sumTotalPaid { get; set; }
    private decimal _sumTotalAwaiting { get; set; }
    private double _sumTotalHours { get; set; }

    public AdminTimesheetPanelLeft()
    {
        _payStatuses = (PayStatus[])Enum.GetValues(typeof(PayStatus));
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRangeModel = _dateTimeService.GetCurrentMonth();
        _timesheetStateService.OnStateChange += StateHasChanged;
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _spinnerService.ShowSpinner();
                await RefreshList();
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

    private async Task FetchDataAsync()
    {
        try
        {
            _companies = await _companyService.GetRecordsActive();

            _timesheets = await _timesheetService.GetRecordsByDateRange(_dateTimeRangeModel);
            _sumTotalAwaiting = await _timesheetService.GetSumTotalAwaiting();
            _sumTotalPaid = await _timesheetService.GetSumTotalPaid();
            _sumTotalHours = await _timesheetService.GetSumTotalHours();

            _timesheetModelStateContainerDTO.Timesheets = _timesheets;
            _timesheetModelStateContainerDTO.TotalAwaiting = _sumTotalAwaiting;
            _timesheetModelStateContainerDTO.TotalPaid = _sumTotalPaid;
            _timesheetModelStateContainerDTO.TotalHours = _sumTotalHours;

            _timesheetStateService.SetValue(_timesheetModelStateContainerDTO);

            await OnStateContainerSetValue.InvokeAsync();

            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
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
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdatePayStatusAsync(TimesheetModelListDTO timesheetModelListDTO, int payStatus)
    {
        try
        {
            TimesheetModel timesheetModel = new()
            {
                Id = timesheetModelListDTO.Id,
                PayStatus = payStatus
            };

            await _timesheetService.UpdateRecordPayStatus(timesheetModel);
            await RefreshList();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task RefreshList()
    {
        await FetchDataAsync();
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
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task RefreshListFromDropdownDateRange()
    {
        await RefreshList();
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await Task.CompletedTask;
    }

    private async Task RefreshListFromCompanyFilter()
    {
        await Task.CompletedTask;
    }

    private string UpdatePayStatusTitle(int payStatus)
    {
        var payStatusTitle = _payStatuses[payStatus];
        return payStatusTitle.ToString();
    }

    private Theme UpdatePayStatusButtonColor(int payStatus)
    {
        if (payStatus == (int)PayStatus.Paid)
        {
            return Theme.Success;
        }

        return Theme.Light;
    }

    public void Dispose()
    {
        _timesheetStateService.OnStateChange -= StateHasChanged;
    }
}
