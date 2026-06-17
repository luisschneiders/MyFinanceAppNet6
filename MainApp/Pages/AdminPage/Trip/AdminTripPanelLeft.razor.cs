using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Trip;

public partial class AdminTripPanelLeft : ComponentBase
{
    [Inject]
    private ITripService<TripModel> _tripService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDropdownDateMonthYearService _dropdownDateMonthYearService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ICalendarViewService _calendarViewService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    /*
     * Add component reference
     */
    private AdminTripOffCanvas _setupOffCanvas { get; set; } = new();
    private AdminTripModal _setupModal { get; set; } = new();
    private AdminTripModalFilter _setupFilterModal { get; set; } = new();
    private AdminTripModalPrinter _setupPrinterModal { get; set; } = new();

    private DateTimeRange _dateRange { get; set; } = new();
    private List<TripByVehicleGroupDTO> _tripsListView { get; set; } = new();
    private List<TripCalendarDTO> _tripsCalendarView { get; set; } = new();
    private MultiFilterTripDTO _multiFilterTripDTO { get; set; } = new();
    private DateTimeRange _dateCalendar { get; set; } = new();
    private string _viewType { get; set; } = ViewType.Calendar.ToString();
    private string _dropdownDateCalendarLabel { get; set; } = Label.AppNoDateAssigned;
    private PayStatus[] _payStatuses { get; set; } = default!;
    private TripCategory[] _tripCategories { get; set; } = default!;
    private decimal _sumByDateRange { get; set; }
    private DateTime[][] _weeks { get; set; } = default!;
    private string _dropdownDateRangeLabel { get; set; } = Label.AppNoDateAssigned;
    private bool _isLoading { get; set; } = true;
    private bool _isLoadingView { get; set; } = true;

    public AdminTripPanelLeft()
    {
        _payStatuses = (PayStatus[])Enum.GetValues(typeof(PayStatus));
        _tripCategories = (TripCategory[])Enum.GetValues(typeof(TripCategory));
    }

    protected async override Task OnInitializedAsync()
    {
        _dateRange = _dateTimeService.GetCurrentMonth();
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(_dateRange);

        _dateCalendar = _dateTimeService.GetCurrentMonth();
        _dropdownDateCalendarLabel = await _dropdownDateMonthYearService.UpdateLabel(_dateCalendar);

        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                string tripView = await _tripService.GetLocalStorageViewType();

                if (string.IsNullOrEmpty(tripView) == false)
                {
                    _viewType = tripView;
                }

                await FetchDataAsync();
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
            if (_viewType == ViewType.Calendar.ToString())
            {
                _multiFilterTripDTO.DateTimeRange = _dateCalendar;
                _tripsCalendarView = await _tripService.GetRecordsCalendarView(_multiFilterTripDTO);
                _weeks = await _calendarViewService.Build(_dateCalendar);
            }
            else if (_viewType == ViewType.List.ToString())
            {
                _multiFilterTripDTO.DateTimeRange = _dateRange;
                _tripsListView = await _tripService.GetRecordsListView(_multiFilterTripDTO);
            }

            _sumByDateRange = await _tripService.GetSumByDateRange();
            _isLoadingView = false;
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoadingView = false;
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async void UpdateUIVIew(ViewType viewType)
    {
        _viewType = viewType.ToString();

        await _tripService.SetLocalStorageViewType(_viewType);

        await FetchDataAsync();

        await InvokeAsync(StateHasChanged);
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }
    
    private async Task EditRecordAsync(TripListDTO tripModel)
    {
        try
        {
            await _setupOffCanvas.EditRecordOffCanvasAsync(tripModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task PrintAsync()
    {
        try
        {
            PrintTripDTO printTripDTO = new()
            {
                DateTimeRange = _dateRange,
                TripsByGroup = _tripsListView,
                SumByDateRange = _sumByDateRange,
            };

            await _setupPrinterModal.OpenModalAsync(printTripDTO);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task ArchiveRecordAsync(TripListDTO model)
    {
        try
        {
            await _setupModal.OpenModalAsync(model.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdatePayStatusAsync(TripListDTO tripListDTO, int payStatus)
    {
        try
        {
            TripModel tripModel = new()
            {
                Id = tripListDTO.Id,
                PayStatus = payStatus
            };

            await _tripService.UpdateRecordPayStatus(tripModel);
            await RefreshList();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateTripCategoryAsync(TripListDTO tripListDTO, ulong tripCategory)
    {
        try
        {
            TripModel tripModel = new()
            {
                Id = tripListDTO.Id,
                TCategoryId = tripCategory
            };

            await _tripService.UpdateRecordTripCategory(tripModel);
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

    private async Task RefreshFilterList(MultiFilterTripDTO multiFilterTripDTO)
    {
        _multiFilterTripDTO = multiFilterTripDTO;
        _multiFilterTripDTO.IsFilterChanged = true;

        await FetchDataAsync();

        await Task.CompletedTask;
    }

    private async Task DropdownDateRangeRefresh(DateTimeRange dateTimeRange)
    {
        _dateRange = dateTimeRange;
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast(Label.AppMessageDateRangeChanged, Theme.Info);

        await RefreshList();

        await Task.CompletedTask;
    }

    private string UpdatePayStatusTitle(int id)
    {
        var title = _payStatuses[id];

        return title.ToString();
    }

    private string UpdateTripCategoryTitle(ulong id)
    {
        string? title;

        if (id == (int)TripCategory.NotSpecified)
        {
            title = Label.AppNotSpecified;
        }
        else
        {
            title = _enumHelper.GetDescription(_tripCategories[id]);
        }

        return title;
    }

    private Theme UpdatePayStatusButton(int id)
    {
        if (id == (int)PayStatus.Paid)
        {
            return Theme.Success;
        }

        return Theme.Light;
    }

    private Theme UpdateTripCategoryButton(ulong id)
    {
        if (id == (int)TripCategory.NotSpecified)
        {
            return Theme.Warning;
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
        _multiFilterTripDTO = new();

        await FetchDataAsync();

        await Task.CompletedTask;
    }

    private bool IsFilterApplied()
    {
        if (_multiFilterTripDTO.VehicleId.Count > 0 || _multiFilterTripDTO.TCategoryId.Count > 0)
        {
            return true;
        }
        else{
            return false;
        }
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
}
