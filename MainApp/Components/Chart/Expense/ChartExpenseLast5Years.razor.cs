using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyFinanceAppLibrary.Models;

namespace MainApp.Components.Chart.Expense;

public partial class ChartExpenseLast5Years : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    private List<ExpenseLast5YearsGraphDTO> _expenseLast5YearsGraphDTO { get; set; } = new();

    private ChartConfigData _chartConfigData { get; set; } = new();
    private ChartConfigDataset _chartConfigDataset { get; set; } = new();
    private List<string> _chartBackgroundColors { get; set; } = new();
    private List<string> _chartBorderColors { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;

    public ChartExpenseLast5Years()
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
        _chartBackgroundColors.Add(BackgroundColor.Blue);
        _chartBackgroundColors.Add(BackgroundColor.Yellow);

        _chartBorderColors.Add(BorderColor.Orange);
        _chartBorderColors.Add(BorderColor.Purple);
        _chartBorderColors.Add(BorderColor.Green);
        _chartBorderColors.Add(BorderColor.Blue);
        _chartBorderColors.Add(BorderColor.Yellow);

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _expenseLast5YearsGraphDTO = await _expenseService.GetRecordsLast5Years();

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
            if (_expenseLast5YearsGraphDTO.Count > 0)
            {

                foreach (var (item, index) in _expenseLast5YearsGraphDTO.Select((value, index) => (value, index)))
                {
                    _chartConfigDataset.Label = "Total expenses";
                    _chartConfigDataset.BackgroundColor.Add(_chartBackgroundColors[index]);
                    _chartConfigDataset.BorderColor.Add(_chartBorderColors[index]);
                    _chartConfigDataset.Data.Add(item.TotalAmount.ToString());
                    _chartConfigData.Labels.Add($"{item.YearNumber}");
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
