using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Chart;

public class ChartService : IChartService, IAsyncDisposable
{
    [Inject]
    private IJSRuntime _js { get; set; } = default!;

    private IJSObjectReference _chartModule = default!;
    private IJSObjectReference _chartObjectReference = default!;

    private List<string> _chartData { get; set; } = new();

    public ChartService(IJSRuntime JS)
    {   
        _js = JS;
    }

    public async Task<IJSObjectReference> InvokeChartModule()
    {
        _chartModule = await _js.InvokeAsync<IJSObjectReference>("import", "./Components/Chart/Chart.razor.js");
        return await Task.FromResult(_chartModule);
    }

    public async Task<IJSObjectReference> SetupChartModule(string id, ChartConfig chartConfig)
    {
        _chartObjectReference = await _chartModule.InvokeAsync<IJSObjectReference>("setupChart", id, chartConfig);
        return await Task.FromResult(_chartObjectReference);
    }

    public async Task<IJSObjectReference> GetChartObjectReference()
    {
        return await Task.FromResult(_chartObjectReference);
    }

    public async Task UpdateChartData(IJSObjectReference chartObjectReference, ChartConfigData chartData)
    {
        _chartObjectReference = chartObjectReference;
        if (chartObjectReference is not null)
        {
            await _chartModule.InvokeVoidAsync("updateChartData", chartObjectReference, chartData);
        }
        await Task.CompletedTask;
    }

    public async Task RemoveChartData(IJSObjectReference chartObjectReference)
    {
        _chartObjectReference = chartObjectReference;
        if (chartObjectReference is not null)
        {
            await _chartModule.InvokeVoidAsync("removeChartData", chartObjectReference);
        }
        await Task.CompletedTask;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_chartModule is not null)
        {
            await _chartModule.DisposeAsync();
        }

        if (_chartObjectReference is not null)
        {
            await _chartObjectReference.DisposeAsync();
        }
    }
}
