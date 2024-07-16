﻿using Microsoft.AspNetCore.Components;
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
    private ICalendarViewService _calendarViewService { get; set; } = default!;

    [Inject]
    private ICompanyService<CompanyModel> _companyService { get; set; } = default!;

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private ToastService _toastService { get; set; } = new();

    // [Inject]
    // private TimesheetStateService _timesheetStateService { get; set; } = default!;

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDropdownDateMonthYearService _dropdownDateMonthYearService { get; set; } = default!;

    //TODO: replace ILocalStorageService with IAppSettingsService
    [Inject]
    private ILocalStorageService _localStorageService { get; set; } = default!;


    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    // [Parameter]
    // public EventCallback OnStateContainerSetValue { get; set; }

    /*
     * Add OffCanvas component reference
     */
    private AdminTimesheetOffCanvas _setupOffCanvas { get; set; } = new();

    private DateTimeRange _dateRange { get; set; } = new();
    private List<CompanyModel> _companies { get; set; } = new();
    // private TimesheetStateContainerDTO _timesheetStateContainerDTO { get; set; } = new();
    private PayStatus[] _payStatuses { get; set; } = default!;
    private string _dropdownDateRangeLabel { get; set; } = Label.NoDateAssigned;
    private bool _isLoading { get; set; } = true;
    private AdminTimesheetFilterModal _setupFilterModal { get; set; } = new();
    private FilterTimesheetDTO _filterTimesheetDTO { get; set; } = new();
    private List<TimesheetByCompanyGroupDTO> _timesheetListView { get; set; } = new();
    private List<TimesheetCalendarDTO> _timesheetCalendarView { get; set; } = new();
    private DateTimeRange _dateCalendar { get; set; } = new();
    private string _viewType { get; set; } = ViewType.Calendar.ToString();
    private string _dropdownDateCalendarLabel { get; set; } = Label.NoDateAssigned;
    private DateTime[][] _weeks { get; set; } = default!;
    private decimal _sumByDateRange { get; set; }

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
                _spinnerService.ShowSpinner();
                string timesheetView = await GetLocalStorageTimesheetViewAsync();

                if (string.IsNullOrEmpty(timesheetView) == false)
                {
                    _viewType = timesheetView;
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

    private async Task<string> GetLocalStorageTimesheetViewAsync()
    {
        string? localStorage = await _localStorageService.GetAsync<string>(LocalStorage.AppTimesheetView);

        return await Task.FromResult(localStorage!);
    }

    private async Task FetchDataAsync()
    {
        try
        {
            if (_viewType == ViewType.Calendar.ToString())
            {
                _filterTimesheetDTO.DateTimeRange = _dateCalendar;
                _timesheetCalendarView = await _timesheetService.GetRecordsCalendarView(_filterTimesheetDTO);
                _weeks = await _calendarViewService.Build(_dateCalendar);
            }
            else if (_viewType == ViewType.List.ToString())
            {
                _companies = await _companyService.GetRecords();

                _filterTimesheetDTO.DateTimeRange = _dateRange;
                _timesheetListView = await _timesheetService.GetRecordsListView(_filterTimesheetDTO);

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


            _sumByDateRange = await _timesheetService.GetSumByDateRange();
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

        await _localStorageService.SetAsync<string>(LocalStorage.AppTimesheetView, _viewType);
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

    private async Task RefreshList()
    {
        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private async Task RefreshFilterList(FilterTimesheetDTO filterTimesheetDTO)
    {
        _filterTimesheetDTO = filterTimesheetDTO;
        _filterTimesheetDTO.IsFilterChanged = true;

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

    private async Task RefreshDropdownDateRange(DateTimeRange dateTimeRange)
    {
        _dateRange = dateTimeRange;
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);

        await RefreshList();

        await Task.CompletedTask;
    }

    private async Task RefreshDropdownDateMonthYear(DateTimeRange dateTimeRange)
    {
        _dateCalendar = dateTimeRange;
        _dropdownDateCalendarLabel = await _dropdownDateMonthYearService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);

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
        _filterTimesheetDTO = new();

        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private bool IsFilterApplied()
    {
        if (_filterTimesheetDTO.CompanyId != 0)
        {
            return true;
        }
        else{
            return false;
        }
    }

    // public void Dispose()
    // {
    //     _timesheetStateService.OnStateChange -= StateHasChanged;
    // }
}
