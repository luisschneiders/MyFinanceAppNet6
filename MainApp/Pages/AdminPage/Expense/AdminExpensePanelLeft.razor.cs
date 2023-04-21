using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Pages.AdminPage.Transaction;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpensePanelLeft : ComponentBase
{
    [Inject]
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDropdownDateMonthYearService _dropdownDateMonthYearService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ICalendarViewService _calendarViewService { get; set; } = default!;

    private DateTimeRange _dateRange { get; set; } = new();
    private DateTimeRange _dateCalendar { get; set; } = new();


    /*
     * Add OffCanvas component reference
     */
    private AdminExpenseOffCanvas _setupOffCanvas { get; set; } = new();

    /*
     * Add Modal component reference
     */
    private AdminExpenseModal _setupModal { get; set; } = new();

    private List<ExpenseByCategoryGroupDTO> _expensesByGroup { get; set; } = new();
    private List<ExpenseListDTO> _expensesByMonthYear { get; set; } = new();

    private decimal _expensesTotal { get; set; } = 0;

    private string _dropdownDateRangeLabel { get; set; } = Label.NoDateAssigned;
    private string _dropdownDateCalendarLabel { get; set; } = Label.NoDateAssigned;

    private bool _isLoading { get; set; } = true;

    private ViewType _viewType { get; set; } = ViewType.Calendar;

    private int[][] _weeks { get; set; } = default!;

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
                _spinnerService.ShowSpinner();
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
            if (_viewType == ViewType.Calendar)
            {
                _expensesByMonthYear = await _expenseService.GetRecordsByDateRange(_dateCalendar);
                _weeks = await _calendarViewService.Build(_dateCalendar);
            }
            else if (_viewType == ViewType.List)
            {
                _expensesByGroup = await _expenseService.GetRecordsByGroupAndDateRange(_dateRange);
                _expensesTotal = await _expenseService.GetRecordsByDateRangeSum();
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
        _viewType = viewType;
        await FetchDataAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
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

    private async Task RefreshList()
    {
        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private async Task DropdownDateRangeRefresh(DateTimeRange dateTimeRange)
    {
        _dateRange = dateTimeRange;
        _dropdownDateRangeLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await RefreshList();
        await Task.CompletedTask;
    }

    private async Task DropdownDateMonthYearRefresh(DateTimeRange dateTimeRange)
    {
        _dateCalendar = dateTimeRange;
        _dropdownDateCalendarLabel = await _dropdownDateMonthYearService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await RefreshList();
        await Task.CompletedTask;
    }

}
