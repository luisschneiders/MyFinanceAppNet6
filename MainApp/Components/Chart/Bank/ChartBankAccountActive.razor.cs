using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Chart.Bank;

public partial class ChartBankAccountActive : ComponentBase
{
    [Inject]
    private IChartBankService _chartBankService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private ISpinnerService _spinnerService { get; set; } = default!;

    [Parameter]
    public ChartType ChartType { get; set; } = ChartType.polarArea;

    private ChartConfigData _chartConfigData { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;
    private string _chartIcon { get; set; } = string.Empty;

    public ChartBankAccountActive()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _spinnerService.ShowSpinner();

// TODO: Create a service/helper for dinamic icons based on ChartType
                if (ChartType == ChartType.bar)
                {
                    _chartIcon = "bi-bar-chart-line";
                }
                else if (ChartType == ChartType.line)
                {
                    _chartIcon = "bi bi-graph-up";
                }
                else
                {
                    _chartIcon = "bi bi-pie-chart";
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
        _chartConfigData = await _chartBankService.ConfigDataAccountActive();
        _isLoading = false;

        await InvokeAsync(StateHasChanged);

        await Task.CompletedTask;
    }

    private async Task SetChartObjectReference(IJSObjectReference chartObjectReference)
    {
        _chartObjectReference = chartObjectReference;
        await Task.CompletedTask;
    }
}
