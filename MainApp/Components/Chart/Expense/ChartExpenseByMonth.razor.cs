using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Chart.Expense;

public partial class ChartExpenseByMonth : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    private IChartExpenseService _chartExpenseService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDropdownMultiSelectService _dropDownMultiSelectService { get;set; } = default!;

    [Inject]
    private IExpenseCategoryService<ExpenseCategoryModel> _expenseCategoryService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public ChartType ChartType { get; set; } = ChartType.line; // Or ChartType.line

    private DateTimeRange _dateTimeRange { get; set; } = new();
    private ChartConfigData _chartConfigData { get; set; } = new();
    private FilterModel _filterModel { get; set; } = new();
    private ExpenseCategoryModel _expenseCategory { get; set; } = new();
    private List<CheckboxItemModel> _expenseCategories { get; set; } = new();
    private MultiFilterExpenseByMonthDTO _multiFilterExpenseByMonthDTO { get; set; } = new();
    private string _chartIcon { get; set; } = string.Empty;
    private string _dropdownLabelDate { get; set; } = Label.AppNoDateAssigned;
    private bool _isDisabled { get; set; } = false;
    private bool _isLoading { get; set; } = true;
    private IJSObjectReference _chartObjectReference = default!;
    private string _searchQueryExpense = string.Empty;
    private bool _selectAllCheckedExpense = false;
    private List<CheckboxItemModel> _filteredExpenseCategories => 
        string.IsNullOrWhiteSpace(_searchQueryExpense) 
            ? _expenseCategories 
            : _expenseCategories.Where(ec => ec.Description.Contains(_searchQueryExpense, StringComparison.OrdinalIgnoreCase)).ToList();

    public ChartExpenseByMonth()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentYear();
        _dropdownLabelDate = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);

        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                if (ChartType == ChartType.bar)
                {
                    _chartIcon = "bi-bar-chart-line";
                }
                else
                {
                    _chartIcon = "bi bi-graph-up";
                }

                await FetchExpenseCategoryAsync();
                await SetChartConfigDataAsync();
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }

        await Task.CompletedTask;
    }

    private async Task SetChartConfigDataAsync()
    {
        _multiFilterExpenseByMonthDTO.DateTimeRange = _dateTimeRange;

        _chartConfigData = await _chartExpenseService.ConfigDataByMonth(_multiFilterExpenseByMonthDTO);
        _isLoading = false;

        await InvokeAsync(StateHasChanged);

        await Task.CompletedTask;
    }

    private async Task SetChartObjectReference(IJSObjectReference chartObjectReference)
    {
        _chartObjectReference = chartObjectReference;
        await Task.CompletedTask;
    }

    private async Task FetchExpenseCategoryAsync()
    {
        try
        {
            _expenseCategories = await _expenseCategoryService.GetRecordsForFilter();
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
        _dropdownLabelDate = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);

        await ResetChartDefaults();
        await RefreshChart();

        _toastService.ShowToast("Date range has changed!", Theme.Info);

        await Task.CompletedTask;
    }

    private async Task ResetChartDefaults()
    {
        _chartConfigData = new();

        await Task.CompletedTask;
    }

    private async void OnCheckboxChangedExpense(ChangeEventArgs e, ulong id)
    {
        _isDisabled = true;

        CheckboxItemModel expense = _expenseCategories.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterExpenseByMonthDTO.ECategoryId.Add(id);
            expense.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterExpenseByMonthDTO.ECategoryId.Remove(id);
            expense.IsChecked = false;
        }

        _multiFilterExpenseByMonthDTO.IsFilterChanged = true;

        await RefreshChart();

        _isDisabled = false;

        await InvokeAsync(StateHasChanged);

        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterExpenseCategory()
    {
        _multiFilterExpenseByMonthDTO.ECategoryId = new();
        _expenseCategories = await _dropDownMultiSelectService.UncheckAll(_expenseCategories);

        await RefreshChart();

        await Task.CompletedTask;
    }

    private async Task RefreshChart()
    {
        await SetChartConfigDataAsync();
        await _chartService.UpdateData(_chartObjectReference, _chartConfigData);
    }

    private async void ToggleSelectAllExpense(ChangeEventArgs e)
    {
        _isDisabled = true;

        _selectAllCheckedExpense = (bool)e.Value!;

        foreach (var expenseCategory in _expenseCategories)
        {
            expenseCategory.IsChecked = _selectAllCheckedExpense;

            if (e.Value is true)
            {
                _multiFilterExpenseByMonthDTO.ECategoryId.Add(expenseCategory.Id);
                expenseCategory.IsChecked = true;
            }
            else if (e.Value is false)
            {
                _multiFilterExpenseByMonthDTO.ECategoryId.Remove(expenseCategory.Id);
                expenseCategory.IsChecked = false;
            }

        }

        _multiFilterExpenseByMonthDTO.IsFilterChanged = true;

        await RefreshChart();
        
        _isDisabled = false;

        await InvokeAsync(StateHasChanged);

        await Task.CompletedTask;
    }
}
