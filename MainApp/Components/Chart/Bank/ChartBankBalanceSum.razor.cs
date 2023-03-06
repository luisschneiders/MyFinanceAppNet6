using MainApp.Components.Spinner;
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

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    private ChartConfigData _chartConfigData { get; set; } = new();
    private ChartConfigDataset _chartConfigDataset { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    private BankModelBalanceSumDTO _bankModelBalanceSumDTO { get; set; } = new();

    private IJSObjectReference _chartObjectReference = default!;

    public ChartBankBalanceSum()
    {
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

    private async Task FetchDataAsync()
    {
        try
        {
            _bankModelBalanceSumDTO = await _bankService.GetBankBalancesSum();
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
            if (_bankModelBalanceSumDTO is not null)
            {
                _chartConfigData.Labels.Add(Graphic.BankBalanceInitialSum);
                _chartConfigData.Labels.Add(Graphic.BankBalanceCurrentSum);

                _chartConfigDataset.Label = "Balances";

                _chartConfigDataset.BackgroundColor.Add(BackgroundColor.Gray);
                _chartConfigDataset.BackgroundColor.Add(BackgroundColor.Green);

                _chartConfigDataset.BorderColor.Add(BorderColor.Gray);
                _chartConfigDataset.BorderColor.Add(BorderColor.Green);

                _chartConfigDataset.Data.Add(_bankModelBalanceSumDTO.BankTotalInitialBalance.ToString());
                _chartConfigDataset.Data.Add(_bankModelBalanceSumDTO.BankTotalCurrentBalance.ToString());

                _chartConfigData.Datasets.Add(_chartConfigDataset);

                //_chartObjectReference = await _chartService.GetChartObjectReference();
                //await _chartService.UpdateChartData(_chartObjectReference, _chartConfigData);
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
        await _chartService.UpdateChartData(chartObjectReference, _chartConfigData);
        await Task.CompletedTask;
    }
}
