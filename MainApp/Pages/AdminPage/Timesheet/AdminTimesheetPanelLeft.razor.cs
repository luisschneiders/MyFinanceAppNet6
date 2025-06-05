using Microsoft.AspNetCore.Components;
using MainApp.Components.Toast;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetPanelLeft : ComponentBase
{
    [Inject]
    private ITimesheetService<TimesheetModel> _timesheetService { get; set; } = default!;

    [Inject]
    private IShiftService<ShiftModel> _shiftService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ICalendarViewService _calendarViewService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    // [Inject]
    // private TimesheetStateService _timesheetStateService { get; set; } = default!;

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDropdownDateMonthYearService _dropdownDateMonthYearService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    // [Parameter]
    // public EventCallback OnStateContainerSetValue { get; set; }

    /*
     * Add OffCanvas component reference
     */
    private AdminTimesheetOffCanvas _setupOffCanvas { get; set; } = new();

    private DateTimeRange _dateRange { get; set; } = new();
    // private TimesheetStateContainerDTO _timesheetStateContainerDTO { get; set; } = new();
    private PayStatus[] _payStatuses { get; set; } = default!;
    private string _dropdownDateRangeLabel { get; set; } = Label.AppNoDateAssigned;
    private bool _isLoading { get; set; } = true;
    private AdminTimesheetModalFilter _setupFilterModal { get; set; } = new();
    private AdminTimesheetModalShift _setupShiftModal { get; set; } = new();
    private AdminTimesheetModalCalculator _setupCalculatorModal { get; set; } = new();
    private MultiFilterTimesheetDTO _multiFilterTimesheetDTO { get; set; } = new();
    private List<TimesheetByCompanyGroupDTO> _timesheetListView { get; set; } = new();
    private List<TimesheetCalendarDTO> _timesheetCalendarView { get; set; } = new();
    private List<ShiftListDTO> _shiftCalendarView { get; set; } = new();
    private DateTimeRange _dateCalendar { get; set; } = new();
    private string _viewType { get; set; } = ViewType.Calendar.ToString();
    private string _dropdownDateCalendarLabel { get; set; } = Label.AppNoDateAssigned;
    private DateTime[][] _weeks { get; set; } = default!;
    private TimesheetTotal _timesheetTotal{ get; set; } = new();
    private List<TableColumn> _tableColumns { get; set;} = new();
    private bool _isLoadingView { get; set; } = true;
    
    public AdminTimesheetPanelLeft()
    {
        _payStatuses = (PayStatus[])Enum.GetValues(typeof(PayStatus));
    }

    protected async override Task OnInitializedAsync()
    {
        _dateRange = _dateTimeService.GetCurrentMonth();
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(_dateRange);

        _dateCalendar = _dateTimeService.GetCurrentMonth();
        _dropdownDateCalendarLabel = await _dropdownDateMonthYearService.UpdateLabel(_dateCalendar);

        // _timesheetStateService.OnStateChange += StateHasChanged;

        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                string timesheetView = await _timesheetService.GetLocalStorageViewType();
                List<TableColumn> tableColumns = await _timesheetService.GetLocalStorageTableColumns();

                if (string.IsNullOrEmpty(timesheetView) == false)
                {
                    _viewType = timesheetView;
                }

                if (tableColumns.Count > 0)
                {
                    _tableColumns = tableColumns;
                }

                await FetchDataAsync();
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
            if (_viewType == ViewType.Calendar.ToString())
            {
                _multiFilterTimesheetDTO.DateTimeRange = _dateCalendar;
                _timesheetCalendarView = await _timesheetService.GetRecordsCalendarView(_multiFilterTimesheetDTO);
                _shiftCalendarView = await _shiftService.GetRecordsByDateRange(_multiFilterTimesheetDTO.DateTimeRange);
                _weeks = await _calendarViewService.Build(_dateCalendar);
            }
            else if (_viewType == ViewType.List.ToString())
            {
                _multiFilterTimesheetDTO.DateTimeRange = _dateRange;
                _timesheetListView = await _timesheetService.GetRecordsListView(_multiFilterTimesheetDTO);
            }
            // List<TimesheetListDTO> timesheets = await _timesheetService.GetRecordsByFilter(_dateRange, _filterCompany);
            // decimal totalAwaiting = await _timesheetService.GetSumTotalAwaiting();
            // decimal totalPaid = await _timesheetService.GetSumTotalPaid();
            // double totalHours = await _timesheetService.GetSumTotalHours();

            // _timesheetStateContainerDTO.Timesheets = timesheets;
            // _timesheetStateContainerDTO.TotalAwaiting = totalAwaiting;
            // _timesheetStateContainerDTO.TotalPaid = totalPaid;
            // _timesheetStateContainerDTO.TotalHours = totalHours;

            // _timesheetStateService.SetValue(_timesheetStateContainerDTO);

            // await OnStateContainerSetValue.InvokeAsync();

            _timesheetTotal = await _timesheetService.GetTotals();
            _isLoadingView = false;
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async void UpdateUIVIew(ViewType viewType)
    {
        _viewType = viewType.ToString();

        await _timesheetService.SetLocalStorageViewType(_viewType);

        await FetchDataAsync();

        await InvokeAsync(StateHasChanged);
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

// TODO
    private async Task UpdateAvailabilityAsync(DateTime date)
    {
        try
        {
            // await _setupShiftModal.OpenModalAsync(date);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger); ;
        }

        await Task.CompletedTask;
    }
    private async Task UpdateShiftAsync(DateTime date)
    {
        try
        {
            await _setupShiftModal.OpenModalAsync(date);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);;
        }

        await Task.CompletedTask;
    }

    private async Task OpenCalculatorAsync()
    {
        try
        {
            await _setupCalculatorModal.OpenModalAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);;
        }

        await Task.CompletedTask;
    }

    private async Task RefreshList()
    {
        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private async Task RefreshFilterList(MultiFilterTimesheetDTO multiFilterTimesheetDTO)
    {
        _multiFilterTimesheetDTO = multiFilterTimesheetDTO;
        _multiFilterTimesheetDTO.IsFilterChanged = true;

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

    private async Task PreviousPeriodAsync(DateTimeRange date, ViewType viewType)
    {
        try
        {
            _isLoadingView = true;

            DateTimeRange previousDate = new();

            previousDate = _dateTimeService.GetPreviousMonth(date);

            switch (viewType)
            {
                case ViewType.Calendar:
                    await RefreshDropdownDateMonthYear(previousDate);
                    break;
                case ViewType.List:
                    await RefreshDropdownDateRange(previousDate);
                    break;
            }

            _isLoadingView = false;
        }
        catch (Exception ex)
        {
            _isLoadingView = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        
        await Task.CompletedTask;
    }

    private async Task NextPeriodAsync(DateTimeRange date, ViewType viewType)
    {
        try
        {
            _isLoadingView = true;

            DateTimeRange nextDate = new();

            nextDate = _dateTimeService.GetNextMonth(date);

            switch (viewType)
            {
                case ViewType.Calendar:
                    await RefreshDropdownDateMonthYear(nextDate);
                    break;
                case ViewType.List:
                    await RefreshDropdownDateRange(nextDate);
                    break;
            }

            _isLoadingView = false;
        }
        catch (Exception ex)
        {
            _isLoadingView = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task RefreshDropdownDateRange(DateTimeRange dateTimeRange)
    {
        _dateRange = dateTimeRange;
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast(Label.AppMessageDateRangeChanged, Theme.Info);

        await RefreshList();

        await Task.CompletedTask;
    }

    private async Task RefreshDropdownDateMonthYear(DateTimeRange dateTimeRange)
    {
        _dateCalendar = dateTimeRange;
        _dropdownDateCalendarLabel = await _dropdownDateMonthYearService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast(Label.AppMessageDateRangeChanged, Theme.Info);

        await RefreshList();
        await Task.CompletedTask;
    }

    private string UpdatePayStatusTitle(int id)
    {
        var title = _payStatuses[id];
        return title.ToString();
    }

    private static Theme UpdatePayStatusButton(int id)
    {
        if (id == (int)PayStatus.Paid)
        {
            return Theme.Success;
        }

        return Theme.Light;
    }

    private async Task ApplyFiltersAsync()
    {
        try
        {
            await _setupFilterModal.OpenModalAsync(IsFilterApplied());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ResetAllFilters()
    {
        _multiFilterTimesheetDTO = new();

        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private bool IsFilterApplied()
    {
        if (_multiFilterTimesheetDTO.CompanyId.Count > 0 || _multiFilterTimesheetDTO.StatusId.Count > 0)
        {
            return true;
        }
        else{
            return false;
        }
    }

    static string FormatTimeSpan(TimeSpan timeSpan)
    {
        // Format the TimeSpan with days, hours, minutes, and seconds
        return $"{timeSpan.Days} days, {timeSpan.Hours} hours, {timeSpan.Minutes} minutes";
    }

    private async void OnCheckboxChangedColumns(ChangeEventArgs e, int id)
    {
        TableColumn timesheetColumn = _tableColumns.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            timesheetColumn.IsChecked = true;
        }
        else if (e.Value is false)
        {
            timesheetColumn.IsChecked = false;
        }

        await _timesheetService.SetLocalStorageTableColumns(_tableColumns);

        await Task.CompletedTask;
    }

    // public void Dispose()
    // {
    //     _timesheetStateService.OnStateChange -= StateHasChanged;
    // }
}
