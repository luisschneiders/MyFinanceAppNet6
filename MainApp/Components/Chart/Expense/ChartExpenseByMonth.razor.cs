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
    private FilterModel _filterExpenseCategoryModel { get; set; } = new();
    private FilterExpenseDTO _filterExpenseDTO { get; set; } = new();
    private ExpenseCategoryModel _filterExpenseCategory { get; set; } = new();
    private List<ExpenseCategoryModel> _expenseCategories { get; set; } = new();
    private string _chartIcon { get; set; } = string.Empty;
    private string _dropdownLabel { get; set; } = Label.NoDateAssigned;
    private string _dropdownFilterLabelExpense { get; set; } = Label.NoFilterAssigned;
    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;
    
    public ChartExpenseByMonth()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentYear();
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);
        _dropdownFilterLabelExpense = await _dropdownFilterService.UpdateLabel(Label.FilterByExpenseCategory);
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _spinnerService.ShowSpinner();

                if (ChartType == ChartType.bar)
                {
                    _chartIcon = "bi-bar-chart-line";
                }
                else
                {
                    _chartIcon = "bi bi-graph-up";
                }

                await FetchExpensesAsync();
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
        _chartConfigData = await _chartExpenseService.ConfigDataExpenseByMonth(_dateTimeRange);
        _isLoading = false;

        await InvokeAsync(StateHasChanged);

        await Task.CompletedTask;
    }

    private async Task SetChartObjectReference(IJSObjectReference chartObjectReference)
    {
        _chartObjectReference = chartObjectReference;
        await Task.CompletedTask;
    }

    private async Task FetchExpensesAsync()
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
        _dateTimeRange = dateTimeRange;
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);

        await ResetChartDefaults();
        await SetChartConfigDataAsync();
        await _chartService.UpdateChartData(_chartObjectReference, _chartConfigData);

        _toastService.ShowToast("Date range has changed!", Theme.Info);

        await Task.CompletedTask;
    }

    private async Task ResetChartDefaults()
    {
        _chartConfigData = new();
        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilterExpenseCategory()
    {
        await RemoveDropdownFilterExpenseCategory();
        
        _toastService.ShowToast("Filter for expense removed!", Theme.Info);

        // await OnSubmitFilterSuccess.InvokeAsync(_filterExpenseDTO);
        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterExpenseCategory()
    {
        _filterExpenseCategory = new();
        _filterExpenseDTO.ECategoryId = 0;
        _filterExpenseCategoryModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabelExpense = await _dropdownFilterService.UpdateLabel(Label.FilterByExpenseCategory);

        await Task.CompletedTask;
    }

    private async Task RefreshDropdownFilterExpenseCategory(ulong id)
    {
        _filterExpenseDTO.ECategoryId = id;
        _filterExpenseCategory = _expenseCategories.First(i => i.Id == id);
        string? expenseName = _filterExpenseCategory.Description.Truncate((int)Truncate.ExpenseCategory);

        _filterExpenseCategoryModel = await _dropdownFilterService.SetModel(_filterExpenseCategory.Id, _filterExpenseCategory.Description);

        _dropdownFilterLabelExpense = await _dropdownFilterService.UpdateLabel(expenseName!);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        // await OnSubmitFilterSuccess.InvokeAsync(_filterExpenseDTO);
        await Task.CompletedTask;
    }

}
