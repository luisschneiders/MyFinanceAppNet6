using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Chart;

public class ChartService : IChartService, IAsyncDisposable
{
    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    private IJSObjectReference _chartModule = default!;
    
    private IJSObjectReference _objectReference = default!;

    public ChartService(IJSRuntime jsRuntime)
    {   
        _jsRuntime = jsRuntime;
    }

    public async Task<IJSObjectReference> InvokeModule()
    {
        _chartModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Chart/Chart.razor.js");
        return await Task.FromResult(_chartModule);
    }

    public async Task<IJSObjectReference> SetupModule(string id, ChartConfig chartConfig, Position position)
    {
        _objectReference = await _chartModule.InvokeAsync<IJSObjectReference>("setupChart", id, chartConfig, position.ToString().ToLower());
        return await Task.FromResult(_objectReference);
    }

    public async Task<IJSObjectReference> GetObjectReference()
    {
        return await Task.FromResult(_objectReference);
    }

    public async Task UpdateData(IJSObjectReference chartObjectReference, ChartConfigData chartData)
    {
        _objectReference = chartObjectReference;
        if (chartObjectReference is not null)
        {
            await _chartModule.InvokeVoidAsync("updateChartData", chartObjectReference, chartData);
        }
        await Task.CompletedTask;
    }

    public async Task RemoveData(IJSObjectReference objectReference)
    {
        _objectReference = objectReference;
        if (objectReference is not null)
        {
            await _chartModule.InvokeVoidAsync("removeChartData", objectReference);
        }
        await Task.CompletedTask;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_chartModule is not null)
        {
            await _chartModule.DisposeAsync();
        }

        if (_objectReference is not null)
        {
            await _objectReference.DisposeAsync();
        }
    }
}
