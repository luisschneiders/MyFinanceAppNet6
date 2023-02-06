using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyFinanceAppLibrary.Models;

namespace MainApp.Components.Chart.Bank;

public partial class ChartBankBalanceSum : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    IBankService<BankModel> _bankService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private List<string> _chartBackgroundColors { get; set; } = new();
    private List<string> _chartBorderColors { get; set; } = new();
    private List<string> _chartLabels { get; set; } = new();
    private List<string> _chartData { get; set; } = new();

    private BankModelBalanceSumDTO _bankModelBalanceSumDTO { get; set; } = new();

    private IJSObjectReference _chartObjectReference = default!;

    public ChartBankBalanceSum()
    {
        _chartLabels.Add(Graphic.BankBalanceInitialSum);
        _chartLabels.Add(Graphic.BankBalanceCurrentSum);

        _chartBackgroundColors.Add(BackgroundColor.Gray);
        _chartBackgroundColors.Add(BackgroundColor.Green);

        _chartBorderColors.Add(BorderColor.Gray);
        _chartBorderColors.Add(BorderColor.Green);
    }

    protected async override Task OnInitializedAsync()
    {
        //await FetchDataAsync();
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await FetchDataAsync();
        }
        await Task.CompletedTask;

    }

    private async Task FetchDataAsync()
    {
        try
        {
            _bankModelBalanceSumDTO = await _bankService.GetBankBalancesSum();
            _chartData.Add(_bankModelBalanceSumDTO.BankTotalInitialBalance.ToString());
            _chartData.Add(_bankModelBalanceSumDTO.BankTotalCurrentBalance.ToString());

            _chartObjectReference = await _chartService.GetChartObjectReference();
            await _chartService.UpdateChartData(_chartObjectReference, _chartData);
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task SetChartObjectReference(IJSObjectReference chartObjectReference)
    {
        await _chartService.UpdateChartData(chartObjectReference, _chartData);
        await Task.CompletedTask;
    }
}
