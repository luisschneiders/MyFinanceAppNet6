using MainApp.Components.Chart;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SetupPage.Bank;

public partial class SetupBankPanelRight : ComponentBase
{
    [Inject]
    IBankService<BankModel> _bankService { get; set; } = default!;

    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();


    private List<string> _chartData { get; set; } = new();
    private BankModelBalanceSumDTO _bankModelBalanceSumDTO { get; set; } = new();

    public SetupBankPanelRight()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _bankModelBalanceSumDTO = await _bankService.GetBankBalancesSum();
            _chartData.Add(_bankModelBalanceSumDTO.BankTotalInitialBalance.ToString());
            _chartData.Add(_bankModelBalanceSumDTO.BankTotalCurrentBalance.ToString());

            var chartObjectReference = await _chartService.GetChartObjectReference();
            await _chartService.UpdateChartData(chartObjectReference, _chartData);
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
