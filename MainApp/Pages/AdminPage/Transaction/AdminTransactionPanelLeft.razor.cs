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
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDropdownDateMonthYearService _dropdownDateMonthYearService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    /*
     * Add component reference
     */
    private AdminTransactionOffCanvas _setupOffCanvas { get; set; } = new();
    private AdminTransactionModal _setupModal { get; set; } = new();
    private AdminTransactionModalFilter _setupModalFilter { get; set; } = new();
    private AdminTransactionModalDetails _setupModalTransactionDetails { get; set; } = new();
    private AdminTransactionModalCalculator _setupModalCalculator { get; set; } = new();
    private AdminTransactionModalInfo _setupModalInfo { get; set; } = new();
    private DateTimeRange _dateRange { get; set; } = new();
    private DateTimeRange _dateCalendar { get; set; } = new();
    private List<TransactionByCategoryGroupDTO> _transactionsListView { get; set; } = new();
    private List<TransactionCalendarDTO> _transactionsCalendarView { get; set; } = new();
    private MultiFilterTransactionDTO _multiFilterTransactionDTO { get; set; } = new();
    private string _viewType { get; set; } = ViewType.Calendar.ToString();
    private string _dropdownDateRangeLabel { get; set; } = Label.AppNoDateAssigned;
    private string _dropdownDateCalendarLabel { get; set; } = Label.AppNoDateAssigned;
    private DateTime[][] _weeks { get; set; } = default!;
    private bool _isLoading { get; set; } = true;
    private bool _isLoadingCalendar { get; set; } = true;

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
                string transactionView = await _transactionService.GetLocalStorageViewType();

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

    private async Task FetchDataAsync()
    {
        try
        {
            if (_viewType == ViewType.Calendar.ToString())
            {
                _multiFilterTransactionDTO.DateTimeRange = _dateCalendar;
                _transactionsCalendarView = await _transactionService.GetRecordsCalendarView(_multiFilterTransactionDTO);
                _weeks = await _calendarViewService.Build(_dateCalendar);
                _isLoadingCalendar = false;
            }
            else if (_viewType == ViewType.List.ToString())
            {
                _multiFilterTransactionDTO.DateTimeRange = _dateRange;
                _transactionsListView = await _transactionService.GetRecordsListView(_multiFilterTransactionDTO);
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

        await _transactionService.SetLocalStorageViewType(_viewType);

        await FetchDataAsync();

        await InvokeAsync(StateHasChanged);
    }

    private async Task AddRecordAsync(DateTime date)
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync(date);
        await Task.CompletedTask;
    }

    private async Task PreviousPeriodAsync(DateTimeRange date)
    {
        try
        {
            _isLoadingCalendar = true;
            DateTimeRange previousDate = new();
            previousDate = _dateTimeService.GetPreviousMonth(date);

            await RefreshDropdownDateMonthYear(previousDate);
            _isLoadingCalendar = false;
        }
        catch (Exception ex)
        {
            _isLoadingCalendar = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        
        await Task.CompletedTask;
    }

    private async Task NextPeriodAsync(DateTimeRange date)
    {
        try
        {
            _isLoadingCalendar = true;
            DateTimeRange nextDate = new();
            nextDate = _dateTimeService.GetNextMonth(date);

            await RefreshDropdownDateMonthYear(nextDate);
            _isLoadingCalendar = false;
        }
        catch (Exception ex)
        {
            _isLoadingCalendar = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ApplyFiltersAsync()
    {
        try
        {
            await _setupModalFilter.OpenModalAsync(IsFilterApplied());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

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

    private async Task OpenCalculatorAsync()
    {
        try
        {
            await _setupModalCalculator.OpenModalAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);;
        }

        await Task.CompletedTask;
    }


    private async Task ViewTransactionsDetailsAsync(DateTime date)
    {
        try
        {
            await _setupModalTransactionDetails.OpenModalAsync(date);
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

    private async Task RefreshFilterList(MultiFilterTransactionDTO multiFilterTransactionDTO)
    {
        _multiFilterTransactionDTO = multiFilterTransactionDTO;
        _multiFilterTransactionDTO.IsFilterChanged = true;

        await FetchDataAsync();
        await Task.CompletedTask;
    }


    private async Task RefreshDropdownDateRange(DateTimeRange dateTimeRange)
    {
        _dateRange = dateTimeRange;
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast(@Label.AppMessageDateRangeChanged, Theme.Info);

        await RefreshList();
        await Task.CompletedTask;
    }

    private async Task RefreshDropdownDateMonthYear(DateTimeRange dateTimeRange)
    {
        _dateCalendar = dateTimeRange;
        _dropdownDateCalendarLabel = await _dropdownDateMonthYearService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast(@Label.AppMessageDateRangeChanged, Theme.Info);

        await RefreshList();
        await Task.CompletedTask;
    }

    private async Task ResetAllFilters()
    {
        _multiFilterTransactionDTO = new();

        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private bool IsFilterApplied()
    {
        if (_multiFilterTransactionDTO.FromBank.Count > 0 || _multiFilterTransactionDTO.TCategoryId.Count > 0)
        {
            return true;
        }
        else{
            return false;
        }
    }

    private async Task OpenSetupOffCanvas(DateTime date)
    {
        await AddRecordAsync(date);
        await Task.CompletedTask;
    }
}
