using MainApp.Components.Dropdown;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyFinanceAppLibrary.Models;

namespace MainApp.Components.Chart.Transaction;

public partial class ChartTransactionIO : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();

    private List<TransactionIOGraphByDateDTO> _transactionIOGraphByDateDTO { get; set; } = new();

    private List<string> _chartLabels { get; set; } = new();

    private TransactionChartData _transactionChartData { get; set; } = new();
    private List<TransactionIOGraphByDateDTO> _incomes { get; set; } = new();
    private List<TransactionIOGraphByDateDTO> _outcomes { get; set; } = new();

    private List<decimal> _incomeChartData { get; set; } = new();
    private List<decimal> _outcomeChartData { get; set; } = new();

    private ChartConfigData _chartConfigData { get; set; } = new();
    private ChartConfigDataset _chartConfigDataset { get; set; } = new();

    private string _dropdownLabel { get; set; } = Label.NoDateAssigned;
    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;

    public ChartTransactionIO()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentYear();
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);
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
        _chartLabels.Add(Months.January.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.February.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.March.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.April.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.May.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.June.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.July.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.August.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.September.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.October.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.November.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        _chartLabels.Add(Months.December.ToString().Truncate((int)Truncate.ShortMonthName, "")!);

        for (int i = 0; i <= 12; ++i)
        {
            _incomeChartData.Add(0);
            _outcomeChartData.Add(0);
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _transactionIOGraphByDateDTO = await _transactionService.GetIOByDateRange(_dateTimeRange);

            _incomes = _transactionIOGraphByDateDTO.Where(t => t.Label == "C").ToList();
            _outcomes = _transactionIOGraphByDateDTO.Where(t => t.Label == "D").ToList();

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
            if (_transactionIOGraphByDateDTO.Count > 0)
            {

                foreach (var (item, index) in _chartLabels.Select((value, index) => (value, index)))
                {
                    var i = index + 1;
                    var record = _incomes.Find(m => m.MonthNumber == i);

                    if (i == record?.MonthNumber)
                    {
                        _incomeChartData[index] = record.TotalAmount;
                    }

                    _transactionChartData.Income.Add(_incomeChartData[index].ToString());
                }

                foreach (var (item, index) in _chartLabels.Select((value, index) => (value, index)))
                {
                    var i = index + 1;
                    var record = _outcomes.Find(m => m.MonthNumber == i);

                    if (i == record?.MonthNumber)
                    {
                        _outcomeChartData[index] = record.TotalAmount;
                    }

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

    private async Task DropdownDateRangeRefresh(DateTimeRange dateTimeRange)
    {
        _dateTimeRange = dateTimeRange;
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await ResetChartDefaults();
        await SetChartDefaults();
        await FetchDataAsync();
        await SetDataAsync();
        await _chartService.UpdateChartData(_chartObjectReference, _chartConfigData);
        await Task.CompletedTask;
    }

    private async Task ResetChartDefaults()
    {
        _transactionIOGraphByDateDTO = new();
        _chartLabels = new();
        _transactionChartData = new();
        _incomes = new();
        _outcomes = new();
        _incomeChartData = new();
        _outcomeChartData = new();
        _chartConfigData = new();
        _chartConfigDataset = new();

        await Task.CompletedTask;
    }
}
