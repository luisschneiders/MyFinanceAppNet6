using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Chart.Expense;

public partial class ChartExpenseLast5Years : ComponentBase
{
    [Inject]
    private IChartExpenseService _chartExpenseService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private ChartConfigData _chartConfigData { get; set; } = new();

    private bool _isLoading { get; set; } = true;

    private IJSObjectReference _chartObjectReference = default!;

    public ChartExpenseLast5Years()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
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
        _isLoading = false;
        _chartConfigData = await _chartExpenseService.ConfigDataLast5Years();
        StateHasChanged();
        await Task.CompletedTask;
    }

    private async Task SetChartObjectReference(IJSObjectReference chartObjectReference)
    {
        _chartObjectReference = chartObjectReference;
        await Task.CompletedTask;
    }
}
