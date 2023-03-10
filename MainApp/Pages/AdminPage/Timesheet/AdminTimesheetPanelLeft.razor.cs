using Microsoft.AspNetCore.Components;
using MainApp.Pages.AdminPage.Timesheet;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.StateServices;
using MainApp.Components.Dropdown;

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

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Parameter]
    public EventCallback OnStateContainerSetValue { get; set; }

    /*
     * Add OffCanvas component reference
     */
    private AdminTimesheetOffCanvas _setupOffCanvas { get; set; } = new();

    private TimesheetModel _timesheetModel { get; set; } = new();
    private DateTimeRange _dateTimeRange { get; set; } = new();

    private List<CompanyModel> _companies { get; set; } = new();
    private TimesheetStateContainerDTO _timesheetStateContainerDTO { get; set; } = new();

    private PayStatus[] _payStatuses { get; set; } = default!;
    private CompanyModel _filterCompany { get; set; } = new();

    private string _dropdownLabel { get; set; } = Label.NoDateAssigned;
    private bool _isDateTimeRangeChanged { get; set; } = false;
    private bool _isLoading { get; set; } = true;

    public AdminTimesheetPanelLeft()
    {
        _payStatuses = (PayStatus[])Enum.GetValues(typeof(PayStatus));
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        _dropdownLabel = await _dropdownDateRangeService.UpdateDropdownLabel(_dateTimeRange);
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

            List<TimesheetListDTO> timesheets = await _timesheetService.GetRecordsByFilter(_dateTimeRange, _filterCompany);
            decimal totalAwaiting = await _timesheetService.GetSumTotalAwaiting();
            decimal totalPaid = await _timesheetService.GetSumTotalPaid();
            double totalHours = await _timesheetService.GetSumTotalHours();

            _timesheetStateContainerDTO.Timesheets = timesheets;
            _timesheetStateContainerDTO.TotalAwaiting = totalAwaiting;
            _timesheetStateContainerDTO.TotalPaid = totalPaid;
            _timesheetStateContainerDTO.TotalHours = totalHours;

            _timesheetStateService.SetValue(_timesheetStateContainerDTO);

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

    private async Task UpdateStatusAsync(TimesheetListDTO timesheetListDTO)
    {
        try
        {
            TimesheetModel timesheetModel = new()
            {
                Id = timesheetListDTO.Id,
                IsActive = timesheetListDTO.IsActive
            };

            await _timesheetService.UpdateRecordStatus(timesheetModel);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdatePayStatusAsync(TimesheetListDTO timesheetListDTO, int payStatus)
    {
        try
        {
            TimesheetModel timesheetModel = new()
            {
                Id = timesheetListDTO.Id,
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

    private async Task EditRecordAsync(TimesheetListDTO timesheetModel)
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

    private async Task DropdownDateRangeRefreshList()
    {
        _dropdownLabel = await _dropdownDateRangeService.UpdateDropdownLabel(_dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        _isDateTimeRangeChanged = true;
        await RefreshList();
        await Task.CompletedTask;
    }

    private async Task FilterCompanyRefreshList(ulong id)
    {
        _filterCompany = _companies.First(i => i.Id == id);
        await RefreshList();
        _toastService.ShowToast("Filter updated!", Theme.Info);
        await Task.CompletedTask;
    }

    private string UpdateFilterCompanyTitleState()
    {
        string? title = _filterCompany?.Description?.Length > 0 ?
            _filterCompany?.Description.ToString().Truncate((int)Truncate.Company) : Label.FilterByCompany;

        return title!;
    }

    private string UpdatePayStatusTitleState(int id)
    {
        var title = _payStatuses[id];
        return title.ToString();
    }

    private Theme UpdatePayStatusButtonState(int id)
    {
        if (id == (int)PayStatus.Paid)
        {
            return Theme.Success;
        }

        return Theme.Light;
    }

    private async Task ResetFilter()
    {
        _filterCompany = new();
        await RefreshList();
        await Task.CompletedTask;
    }

    private async Task ResetDateTimeRange()
    {
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        _dropdownLabel = await _dropdownDateRangeService.UpdateDropdownLabel(_dateTimeRange);
        _isDateTimeRangeChanged = false;
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await RefreshList();
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _timesheetStateService.OnStateChange -= StateHasChanged;
    }
}
