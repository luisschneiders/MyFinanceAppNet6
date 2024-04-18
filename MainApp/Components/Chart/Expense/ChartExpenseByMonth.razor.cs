using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Chart.Expense;

public partial class ChartExpenseByMonth : ComponentBase
{
    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

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
    private IDropdownFilterService _dropdownFilterService { get; set; } = default!;

    [Inject]
    private IExpenseCategoryService<ExpenseCategoryModel> _expenseCategoryService { get; set; } = default!;

    [Parameter]
    public ChartType ChartType { get; set; } = ChartType.line; // Or ChartType.line

    private DateTimeRange _dateTimeRange { get; set; } = new();
    private ChartConfigData _chartConfigData { get; set; } = new();
    private FilterModel _filterModel { get; set; } = new();
    private ExpenseCategoryModel _expenseCategory { get; set; } = new();
    private List<ExpenseCategoryModel> _expenseCategories { get; set; } = new();
    private FilterExpenseByMonthDTO _filterExpenseByMonthDTO { get; set; } = new();
    private string _chartIcon { get; set; } = string.Empty;
    private string _dropdownLabelDate { get; set; } = Label.NoDateAssigned;
    private string _dropdownLabelFilter { get; set; } = Label.NoFilterAssigned;
    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;
    
    public ChartExpenseByMonth()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentYear();
        _dropdownLabelDate = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);
        _dropdownLabelFilter = await _dropdownFilterService.UpdateLabel(Label.FilterByExpenseCategory);
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _spinnerService.ShowSpinner();
                _filterExpenseByMonthDTO.IsDateChanged = true;

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
        _filterExpenseByMonthDTO.DateTimeRange = _dateTimeRange;
        _filterExpenseByMonthDTO.ExpenseCategoryModel = _expenseCategory;

        _chartConfigData = await _chartExpenseService.ConfigDataExpenseByMonth(_filterExpenseByMonthDTO);
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
            _expenseCategories = await _expenseCategoryService.GetRecords();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task DropdownDateRangeRefresh(DateTimeRange dateTimeRange)
    {
        _filterExpenseByMonthDTO.IsDateChanged = true;

        _dateTimeRange = dateTimeRange;
        _dropdownLabelDate = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);

        await ResetChartDefaults();
        await RefreshChart();

        _toastService.ShowToast("Date range has changed!", Theme.Info);

        await Task.CompletedTask;
    }

    private async Task DropdownFilterRefresh(ulong id)
    {
        _expenseCategory = _expenseCategories.First(i => i.Id == id);

        _filterExpenseByMonthDTO.IsFilterChanged = true;
        _filterExpenseByMonthDTO.ExpenseCategoryModel = _expenseCategory;

        string? expenseName = _expenseCategory.Description.Truncate((int)Truncate.ExpenseCategory);

        _filterModel = await _dropdownFilterService.SetModel(_expenseCategory.Id, _expenseCategory.Description);

        _dropdownLabelFilter = await _dropdownFilterService.UpdateLabel(expenseName!);

        await RefreshChart();
        
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await Task.CompletedTask;
    }

    private async Task ResetChartDefaults()
    {
        _chartConfigData = new();
        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilter()
    {
        await RemoveDropdownFilter();
        
        _toastService.ShowToast("Filter for expense removed!", Theme.Info);

        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilter()
    {
        _expenseCategory = new();

        _filterExpenseByMonthDTO.IsFilterChanged = false;
        _filterExpenseByMonthDTO.ExpenseCategoryModel = _expenseCategory;

        _filterModel = await _dropdownFilterService.ResetModel();
        _dropdownLabelFilter = await _dropdownFilterService.UpdateLabel(Label.FilterByExpenseCategory);

        await RefreshChart();

        await Task.CompletedTask;
    }

    private async Task RefreshChart()
    {
        await SetChartConfigDataAsync();
        await _chartService.UpdateChartData(_chartObjectReference, _chartConfigData);

    }
}
