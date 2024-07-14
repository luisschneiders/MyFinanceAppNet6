using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionPanelLeft : ComponentBase
{
    [Inject]
    private ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ICalendarViewService _calendarViewService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDropdownDateMonthYearService _dropdownDateMonthYearService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    //TODO: replace ILocalStorageService with IAppSettingsService
    [Inject]
    private ILocalStorageService _localStorageService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    /*
     * Add OffCanvas component reference
     */
    private AdminTransactionOffCanvas _setupOffCanvas { get; set; } = new();

    /*
     * Add Modal component reference
     */
    private AdminTransactionModal _setupModal { get; set; } = new();

    /*
     * Add Filter Modal component reference
     */
    private AdminTransactionModalFilter _setupFilterModal { get; set; } = new();

    /*
     * Add Expense Details Modal component reference
     */
    private AdminTransactionModalDetails _setupTransactionModalDetails { get; set; } = new();


    private AdminTransactionModalInfo _setupModalInfo { get; set; } = new();
    private DateTimeRange _dateRange { get; set; } = new();
    private DateTimeRange _dateCalendar { get; set; } = new();
    private List<TransactionByCategoryGroupDTO> _transactionsListView { get; set; } = new();
    private List<TransactionCalendarDTO> _transactionsCalendarView { get; set; } = new();
    private FilterTransactionDTO _filterTransactionDTO { get; set; } = new();
    private string _viewType { get; set; } = ViewType.Calendar.ToString();
    private string _dropdownDateRangeLabel { get; set; } = Label.NoDateAssigned;
    private string _dropdownDateCalendarLabel { get; set; } = Label.NoDateAssigned;
    private DateTime[][] _weeks { get; set; } = default!;
    private bool _isLoading { get; set; } = true;

    public AdminTransactionPanelLeft()
    {
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
                _spinnerService.ShowSpinner();
                string transactionView = await GetLocalStorageTransactionViewAsync();

                if (string.IsNullOrEmpty(transactionView) == false)
                {
                    _viewType = transactionView;
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

    private async Task<string> GetLocalStorageTransactionViewAsync()
    {
        string? localStorage = await _localStorageService.GetAsync<string>(LocalStorage.AppTransactionView);

        return await Task.FromResult(localStorage!);
    }

    private async Task FetchDataAsync()
    {
        try
        {
            if (_viewType == ViewType.Calendar.ToString())
            {
                _filterTransactionDTO.DateTimeRange = _dateCalendar;
                _transactionsCalendarView = await _transactionService.GetRecordsCalendarView(_filterTransactionDTO);
                _weeks = await _calendarViewService.Build(_dateCalendar);

            }
            else if (_viewType == ViewType.List.ToString())
            {
                _filterTransactionDTO.DateTimeRange = _dateRange;
                _transactionsListView = await _transactionService.GetRecordsListView(_filterTransactionDTO);
            }

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

        await _localStorageService.SetAsync<string>(LocalStorage.AppTransactionView, _viewType);
        await FetchDataAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task AddRecordAsync(DateTime date)
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync(date);
        await Task.CompletedTask;
    }

    private async Task ArchiveRecordAsync(TransactionListDTO model)
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

    private async Task InfoRecordAsync()
    {
        await _setupModalInfo.OpenModalAsync();
    }

    private async Task ViewTransactionsDetailsAsync(DateTime date)
    {
        try
        {
            await _setupTransactionModalDetails.OpenModalAsync(date);
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

    private async Task RefreshFilterList(FilterTransactionDTO filterTransactionDTO)
    {
        _filterTransactionDTO = filterTransactionDTO;
        _filterTransactionDTO.IsFilterChanged = true;

        await FetchDataAsync();
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

    private async Task ResetAllFilters()
    {
        _filterTransactionDTO = new();

        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private bool IsFilterApplied()
    {
        if (_filterTransactionDTO.FromBank != 0 || _filterTransactionDTO.TCategoryId != 0)
        {
            return true;
        }
        else{
            return false;
        }
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

    private async Task OpenSetupOffCanvas(DateTime date)
    {
        await AddRecordAsync(date);
        await Task.CompletedTask;
    }
}
