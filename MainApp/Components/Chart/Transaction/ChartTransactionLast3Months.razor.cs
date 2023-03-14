using DateTimeLibrary.Models;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyFinanceAppLibrary.Models;

namespace MainApp.Components.Chart.Transaction;

public partial class ChartTransactionLast3Months : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    private List<TransactionIOLast3MonthsGraphDTO> _transactionIOLast3MonthsGraphDTO { get; set; } = new();

    private TransactionChartData _transactionChartData { get; set; } = new();
    private List<TransactionIOLast3MonthsGraphDTO> _incomes { get; set; } = new();
    private List<TransactionIOLast3MonthsGraphDTO> _outcomes { get; set; } = new();

    private List<string> _chartLabels { get; set; } = new();
    private List<decimal> _incomeChartData { get; set; } = new();
    private List<decimal> _outcomeChartData { get; set; } = new();

    private ChartConfigData _chartConfigData { get; set; } = new();
    private ChartConfigDataset _chartConfigDataset { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;


    public ChartTransactionLast3Months()
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
        for (int i = 2; i >= 0; --i)
        {
            // previous 3 months
            _chartLabels.Add($"{DateTime.Now.AddMonths(-(i + 1)).Month}/{DateTime.Now.AddMonths(-(i + 1)).Year}");
            _incomeChartData.Add(0);
            _outcomeChartData.Add(0);
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _transactionIOLast3MonthsGraphDTO = await _transactionService.GetRecordsLast3Months();

            _incomes = _transactionIOLast3MonthsGraphDTO.Where(t => t.Label == "C").ToList();
            _outcomes = _transactionIOLast3MonthsGraphDTO.Where(t => t.Label == "D").ToList();

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
            if (_transactionIOLast3MonthsGraphDTO.Count > 0)
            {
                for (int index = 0; index <= 2; index++)
                {
                    if (_incomes.Count > 0)
                    {
                        _incomeChartData[index] = _incomes[index].TotalAmount;
                    }

                    if (_outcomes.Count > 0)
                    {
                        _outcomeChartData[index] = _outcomes[index].TotalAmount;
                    }

                    _transactionChartData.Income.Add(_incomeChartData[index].ToString());
                    _transactionChartData.Outcome.Add(_outcomeChartData[index].ToString());
                }

                _chartConfigData.Labels = _chartLabels;

                //Incomes data
                _chartConfigDataset.Label = "Incomes";
                _chartConfigDataset.BackgroundColor.Add(BackgroundColor.Blue);
                _chartConfigDataset.BorderColor.Add(BorderColor.Blue);
                _chartConfigDataset.Data = _transactionChartData.Income;

                _chartConfigData.Datasets.Add(_chartConfigDataset);

                //Outcomes data
                _chartConfigDataset = new();

                _chartConfigDataset.Label = "Outcomes";
                _chartConfigDataset.BackgroundColor.Add(BackgroundColor.Red);
                _chartConfigDataset.BorderColor.Add(BorderColor.Red);
                _chartConfigDataset.Data = _transactionChartData.Outcome;

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
