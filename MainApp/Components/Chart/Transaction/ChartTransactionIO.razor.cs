using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Chart.Transaction;

public partial class ChartTransactionIO : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    private IChartTransactionService _chartTransactionService { get; set; } = default!;

    [Inject]
    private ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Parameter]

    public ChartType ChartType { get; set; } = ChartType.bar; // Or ChartType.line

    private DateTimeRange _dateTimeRange { get; set; } = new();

    private ChartConfigData _chartConfigData { get; set; } = new();

    private string _dropdownLabel { get; set; } = Label.NoDateAssigned;
    private string _chartIcon { get; set; } = string.Empty;
    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;

    public ChartTransactionIO()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentYear();
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);

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
        _chartConfigData = await _chartTransactionService.ConfigDataIO(_dateTimeRange);
        _isLoading = false;

        await InvokeAsync(StateHasChanged);

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
}
