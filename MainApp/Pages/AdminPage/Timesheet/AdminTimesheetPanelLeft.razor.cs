using Microsoft.AspNetCore.Components;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;

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

    [Inject]
    private IDropdownFilterService _dropdownFilterService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public EventCallback OnStateContainerSetValue { get; set; }

    /*
     * Add OffCanvas component reference
     */
    private AdminTimesheetOffCanvas _setupOffCanvas { get; set; } = new();

    private DateTimeRange _dateTimeRange { get; set; } = new();

    private List<CompanyModel> _companies { get; set; } = new();
    private TimesheetStateContainerDTO _timesheetStateContainerDTO { get; set; } = new();

    private PayStatus[] _payStatuses { get; set; } = default!;

    private FilterModel _filterModel { get; set; } = new();
    private CompanyModel _filterCompany { get; set; } = new();

    private string _dropdownDateRangeLabel { get; set; } = Label.NoDateAssigned;
    private string _dropdownFilterLabel { get; set; } = Label.NoFilterAssigned;

    private bool _isLoading { get; set; } = true;

    public AdminTimesheetPanelLeft()
    {
        _payStatuses = (PayStatus[])Enum.GetValues(typeof(PayStatus));
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);
        _dropdownFilterLabel = await _dropdownFilterService.UpdateLabel(Label.FilterByCompany);
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

            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _companies = await _companyService.GetRecords();

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

    private async Task DropdownDateRangeRefresh(DateTimeRange dateTimeRange)
    {
        _dateTimeRange = dateTimeRange;
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);

        await RefreshList();

        await Task.CompletedTask;
    }

    private async Task DropdownFilterRefreshCompany(ulong id)
    {
        _filterCompany = _companies.First(i => i.Id == id);
        string? companyName = _filterCompany.Description.Truncate((int)Truncate.Company);

        _filterModel = await _dropdownFilterService.SetModel(_filterCompany.Id, _filterCompany.Description);

        _dropdownFilterLabel = await _dropdownFilterService.UpdateLabel(companyName!);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await RefreshList();

        await Task.CompletedTask;
    }

    private string UpdatePayStatusTitle(int id)
    {
        var title = _payStatuses[id];
        return title.ToString();
    }

    private Theme UpdatePayStatusButton(int id)
    {
        if (id == (int)PayStatus.Paid)
        {
            return Theme.Success;
        }

        return Theme.Light;
    }

    private async Task DropdownFilterReset()
    {
        _filterCompany = new();
        _filterModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabel = await _dropdownFilterService.UpdateLabel(Label.FilterByCompany);
        _toastService.ShowToast("Filter removed!", Theme.Info);
        await RefreshList();
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _timesheetStateService.OnStateChange -= StateHasChanged;
    }
}
