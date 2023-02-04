using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Chart.Bank;

public partial class ChartBankBalanceSum : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Parameter]
    public List<string> Data { get; set; } = new();

    private List<string> _chartBackgroundColors { get; set; } = new();
    private List<string> _chartBorderColors { get; set; } = new();
    private List<string> _chartLabels { get; set; } = new();

    public ChartBankBalanceSum()
    {
        _chartLabels.Add(Graphic.BankBalanceInitialSum);
        _chartLabels.Add(Graphic.BankBalanceCurrentSum);

        _chartBackgroundColors.Add(BackgroundColor.Gray);
        _chartBackgroundColors.Add(BackgroundColor.Green);

        _chartBorderColors.Add(BorderColor.Gray);
        _chartBorderColors.Add(BorderColor.Green);
    }

    private async Task SetChartObjectReference(IJSObjectReference chartObjectReference)
    {
        await _chartService.UpdateChartData(chartObjectReference, Data);
        await Task.CompletedTask;
    }
}
