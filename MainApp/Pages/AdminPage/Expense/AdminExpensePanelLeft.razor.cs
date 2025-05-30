using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpensePanelLeft : ComponentBase
{
    [Inject]
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

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

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    /*
     * Add component reference
     */
    private AdminExpenseOffCanvas _setupOffCanvas { get; set; } = new();
    private AdminExpenseModal _setupModal { get; set; } = new();
    private AdminExpenseModalFilter _setupFilterModal { get; set; } = new();
    private AdminExpenseModalDetails _setupExpenseModalDetails { get; set; } = new();

    private DateTimeRange _dateRange { get; set; } = new();
    private DateTimeRange _dateCalendar { get; set; } = new();
    private List<ExpenseByCategoryGroupDTO> _expensesListView { get; set; } = new();
    private List<ExpenseCalendarDTO> _expensesCalendarView { get; set; } = new();
    private MultiFilterExpenseDTO _multiFilterExpenseDTO { get; set; } = new();
    private string _viewType { get; set; } = ViewType.Calendar.ToString();
    private string _dropdownDateRangeLabel { get; set; } = Label.AppNoDateAssigned;
    private string _dropdownDateCalendarLabel { get; set; } = Label.AppNoDateAssigned;
    private DateTime[][] _weeks { get; set; } = default!;
    private decimal _expensesTotal { get; set; } = 0;
    private bool _isLoading { get; set; } = true;
    private bool _isLoadingView { get; set; } = true;

    public AdminExpensePanelLeft()
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
                string expenseView = await _expenseService.GetLocalStorageViewType();

                if (string.IsNullOrEmpty(expenseView) == false)
                {
                    _viewType = expenseView;
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
                _multiFilterExpenseDTO.DateTimeRange = _dateCalendar;
                _expensesCalendarView = await _expenseService.GetRecordsCalendarView(_multiFilterExpenseDTO);
                _weeks = await _calendarViewService.Build(_dateCalendar);
            }
            else if (_viewType == ViewType.List.ToString())
            {
                _multiFilterExpenseDTO.DateTimeRange = _dateRange;
                _expensesListView = await _expenseService.GetRecordsListView(_multiFilterExpenseDTO);
            }

            _expensesTotal = await _expenseService.GetRecordsByDateRangeSum();
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

        await _expenseService.SetLocalStorageViewType(_viewType);

        await FetchDataAsync();

        await InvokeAsync(StateHasChanged);
    }

    private async Task AddRecordAsync(DateTime date)
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync(date);
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

    private async Task ArchiveRecordAsync(ExpenseListDTO model)
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

    private async Task ViewExpensesDetailsAsync(DateTime date)
    {
        try
        {
            await _setupExpenseModalDetails.OpenModalAsync(date);
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

    private async Task RefreshFilterList(MultiFilterExpenseDTO multiFilterExpenseDTO)
    {
        _multiFilterExpenseDTO = multiFilterExpenseDTO;
        _multiFilterExpenseDTO.IsFilterChanged = true;

        await FetchDataAsync();
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

    private async Task ResetAllFilters()
    {
        _multiFilterExpenseDTO = new();

        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private bool IsFilterApplied()
    {
        if (_multiFilterExpenseDTO.BankId.Count > 0 || _multiFilterExpenseDTO.ECategoryId.Count > 0)
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
