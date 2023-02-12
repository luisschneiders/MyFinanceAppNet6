using Google.Protobuf.WellKnownTypes;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyFinanceAppLibrary.Models;

namespace MainApp.Components.Chart.Bank;

public partial class ChartBankAccountActive : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Inject]
    IBankService<BankModel> _bankService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    private List<string> _chartBackgroundColors { get; set; } = new();
    private List<string> _chartBorderColors { get; set; } = new();
    private List<string> _chartLabels { get; set; } = new();
    private List<string> _chartData { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    private List<BankModel> _banks { get; set; } = new();

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
            _banks = await _bankService.GetRecordsActive();
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
            if (_banks.Count > 0)
            {
                foreach (var bank in _banks)
                {
                    if (bank.CurrentBalance >= 0 && bank.CurrentBalance <= 1000)
                    {
                        _chartBackgroundColors.Add(BackgroundColor.Gray);
                        _chartBorderColors.Add(BorderColor.Gray);
                    }
                    else if (bank.CurrentBalance >= 1000 && bank.CurrentBalance <= 20000)
                    {
                        _chartBackgroundColors.Add(BackgroundColor.Green);
                        _chartBorderColors.Add(BorderColor.Green);
                    }
                    else
                    {
                        _chartBackgroundColors.Add(BackgroundColor.Blue);
                        _chartBorderColors.Add(BorderColor.Blue);
                    }
                    _chartLabels.Add(bank.Description);
                    _chartData.Add(bank.CurrentBalance.ToString());
                }

                var chartObjectReference = await _chartService.GetChartObjectReference();
                await _chartService.UpdateChartData(chartObjectReference, _chartData);
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
        await _chartService.UpdateChartData(chartObjectReference, _chartData);
        await Task.CompletedTask;
    }
}
