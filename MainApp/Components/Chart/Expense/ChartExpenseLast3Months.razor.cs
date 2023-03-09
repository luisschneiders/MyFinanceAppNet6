using DateTimeLibrary.Models;
using MainApp.Components.Chart.Transaction;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyFinanceAppLibrary.Models;

namespace MainApp.Components.Chart.Expense;

public partial class ChartExpenseLast3Months : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    private List<ExpenseLast3MonthsGraphDTO> _expenseLast3MonthsGraphDTO { get; set; } = new();

    private ChartConfigData _chartConfigData { get; set; } = new();
    private ChartConfigDataset _chartConfigDataset { get; set; } = new();
    private List<string> _chartBackgroundColors { get; set; } = new();
    private List<string> _chartBorderColors { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;

    public ChartExpenseLast3Months()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        await SetChartDefaults();
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
                await SetDataAsync();
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }

        await Task.CompletedTask;
    }

    private async Task SetChartDefaults()
    {
        _chartBackgroundColors.Add(BackgroundColor.Orange);
        _chartBackgroundColors.Add(BackgroundColor.Purple);
        _chartBackgroundColors.Add(BackgroundColor.Green);

        _chartBorderColors.Add(BorderColor.Orange);
        _chartBorderColors.Add(BorderColor.Purple);
        _chartBorderColors.Add(BorderColor.Green);

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _expenseLast3MonthsGraphDTO = await _expenseService.GetRecordsLast3Months();

            _isLoading = false;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task SetDataAsync()
    {
        try
        {
            if (_expenseLast3MonthsGraphDTO.Count > 0)
            {

                foreach (var (item, index) in _expenseLast3MonthsGraphDTO.Select((value, index) => (value, index)))
                {
                    _chartConfigDataset.Label = "Total expenses";
                    _chartConfigDataset.BackgroundColor.Add(_chartBackgroundColors[index]);
                    _chartConfigDataset.BorderColor.Add(_chartBorderColors[index]);
                    _chartConfigDataset.Data.Add(item.TotalAmount.ToString());
                    _chartConfigData.Labels.Add($"{item.MonthNumber}/{item.YearNumber}");
                }

                _chartConfigData.Datasets.Add(_chartConfigDataset);
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task SetChartObjectReference(IJSObjectReference chartObjectReference)
    {
        _chartObjectReference = chartObjectReference;
        await Task.CompletedTask;
    }
}
