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

    private ChartConfigData _chartConfigData { get; set; } = new();
    private ChartConfigDataset _chartConfigDataset { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    private List<BankModel> _banks { get; set; } = new();

    private IJSObjectReference _chartObjectReference = default!;

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
                    _chartConfigData.Labels.Add(bank.Description);

                    if (bank.CurrentBalance >= 0 && bank.CurrentBalance <= 1000)
                    {
                        _chartConfigDataset.BackgroundColor.Add(BackgroundColor.Gray);
                        _chartConfigDataset.BorderColor.Add(BorderColor.Gray);
                    }
                    else if (bank.CurrentBalance >= 1000 && bank.CurrentBalance <= 20000)
                    {
                        _chartConfigDataset.BackgroundColor.Add(BackgroundColor.Green);
                        _chartConfigDataset.BorderColor.Add(BorderColor.Green);
                    }
                    else
                    {
                        _chartConfigDataset.BackgroundColor.Add(BackgroundColor.Blue);
                        _chartConfigDataset.BorderColor.Add(BorderColor.Blue);
                    }

                    _chartConfigDataset.Data.Add(bank.CurrentBalance.ToString());
                }

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
